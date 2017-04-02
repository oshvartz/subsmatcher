using System;
using System.IO;
using CookComputing.XmlRpc;

using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API;
using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.Helper;
using SubtitlesMatcher.Infrastructure;
using System.Configuration;
using OpenSubtitlesOrgMatcher;
using OpenSubtitlesOrgMatcher.OpenSubtitlesOrg;

namespace SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg
{
    public class OpenSubtitlesXmlRpc
    {


        public const string OPENSUBTITLES_URI = "http://api.opensubtitles.org/xml-rpc";

        #region Fields (3)

        readonly IOpenSubtitlesDb _client;
        string _loginToken;
        readonly string _movieFileName;

        #endregion Fields

        #region Constructors (1)

        public OpenSubtitlesXmlRpc(string movieFileName)
        {
            _movieFileName = movieFileName;
            _client = XmlRpcProxyGen.Create<IOpenSubtitlesDb>();
            _client.Expect100Continue = false;
            _client.UserAgent = "SubtitleTools";
            _client.Url = OPENSUBTITLES_URI;
        }

        #endregion Constructors

        #region Methods (5)

        // Public Methods (4) 

        public void DownloadAllSubtitles(Action<int> progress, string subLanguageId = "all")
        {
            var info = GetListOfAllSubtitles(progress, subLanguageId);
            if (info == null || info.data == null)
            {
                return;
            }

            foreach (var item in info.data)
            {
                DownloadSubtitle(item.IDSubtitleFile, item.SubFileName, item.LanguageName, progress);
            }
        }

        public void DownloadSubtitle(string id, string subFileName, string lang, Action<int> progress, bool debugMode = false)
        {
            progress(10);

            tryLogin();

            progress(40);

            
            var result = _client.DownloadSubtitles(_loginToken, new[] { id });

            progress(75);

            if (result.status == "200 OK")
            {
                if (result.data == null || result.data.Length == 0) return;

                var gzBase64Data = result.data[0].data;
                if (string.IsNullOrWhiteSpace(gzBase64Data))
                {
                    throw new Exception("Received gzBase64Data is empty.");
                }

                var fileName = string.Format(@"{0}\{1}-{2}-{3}", Path.GetDirectoryName(_movieFileName), id, lang, subFileName);

                if (debugMode)
                {
                    File.WriteAllText(string.Format("{0}.base64gz", fileName), gzBase64Data);
                }

                //from: http://trac.opensubtitles.org/projects/opensubtitles/wiki/XmlRpcIntro
                //it's gzipped without header.
                var gzBuffer = Convert.FromBase64String(gzBase64Data);
                var content = Compression.DecompressGz(gzBuffer);
                
                File.WriteAllBytes(fileName, content);
            }
            else
            {
                throw new Exception(string.Format("Status: {0}. It's not possible to download {1}", result.status, subFileName));
            }

            progress(100);
        }

        public SubtitlesSearchResult GetListOfAllSubtitles(Action<int> progress, string subLanguageId = "all")
        {
            var fileInfo = new MovieFileInfo(_movieFileName, subFileName: string.Empty);
            //get file info            
            var movieHash = fileInfo.MovieHash;
            
            var fileLen = fileInfo.MovieFileLength;

            //login
            progress(25);

            
            tryLogin();

            progress(50);

            //has any subtitle?
            var token = _loginToken;

            var hashInfo = MovieHashCache.Instance.GetMovieCheckHashResult(movieHash);
            if (hashInfo == null)
            {
                 hashInfo = _client.CheckMovieHash2(token, new[] { movieHash });
                 MovieHashCache.Instance.AddMovieCheckHashResultToCache(movieHash, hashInfo);
            }
            var tryConvertToObjArr = hashInfo.data as object[];
            if ((tryConvertToObjArr != null) && (tryConvertToObjArr.Length == 0))
            {
               // throw new Exception("This movie has not any subtitle.");
                progress(100);
                return null;
            }

            progress(75);

            

            //get more info
            SubtitlesSearchResult result;

            try
            {
                result = _client.SearchSubtitles(
                    token,
                    new[] 
                { 
                    new SearchInfo
                    {
                        moviehash = movieHash,
                        sublanguageid = subLanguageId,
                        moviebytesize = fileLen
                    }
                }
                );
            }
            catch (Exception ex)
            {
                
                progress(100);
                return null;
            }

            if (result != null && result.data != null && result.data.Length > 0)
            {
                
            }
            else
            {
                
            }

            progress(100);

            return result;
        }

        public string UploadSubtitle(string subLanguageId, string subFileNamePath, Action<int> progress)
        {
            string finalUrl;
            var fileInfo = new MovieFileInfo(_movieFileName, subFileNamePath);
            //login            
            progress(10);

            
            tryLogin();

            progress(25);

            
            var res = _client.TryUploadSubtitles(_loginToken,
                new[]
                { 
                    new TryUploadInfo
                    {
                       subhash = fileInfo.SubtitleHash,
                       subfilename = fileInfo.SubFileName,
                       moviehash = fileInfo.MovieHash,
                       moviebytesize = fileInfo.MovieFileLength,
                       moviefilename = fileInfo.MovieFileName
                    }
                }
                );

            progress(50);

            if (res.status != "200 OK")
            {
                throw new Exception("Bad response ...");
            }

            if (res.alreadyindb == 0)
            {
                if (res.data == null || res.data.Length == 0)
                {
                    throw new Exception("Bad format ...");
                }

            
                var checkSubHashRes = _client.CheckSubHash(_loginToken, new[] { fileInfo.SubtitleHash });

                progress(75);

                var idSubtitleFile = int.Parse(checkSubHashRes.data[fileInfo.SubtitleHash].ToString());
                if (idSubtitleFile > 0)
                {
                    throw new Exception("Duplicate subHash, alreadyindb.");
                }

            
                //xml-rpc.net dll does not work here so, ...
                var post = PostXml.PostData(
                     OPENSUBTITLES_URI,
                     UploadData.CreateUploadXml(_loginToken,
                     new UploadBaseinfo
                     {
                         idmovieimdb = res.data[0]["IDMovieImdb"].ToString(),
                         sublanguageid = subLanguageId,
                         movieaka = string.Empty,
                         moviereleasename = fileInfo.MovieReleaseName,
                         subauthorcomment = string.Empty
                     },
                     new UploadCDsInfo
                     {
                         moviebytesize = fileInfo.MovieFileLength.ToString(),
                         moviefilename = fileInfo.MovieFileName,
                         moviehash = fileInfo.MovieHash,
                         subfilename = fileInfo.SubFileName,
                         subhash = fileInfo.SubtitleHash,
                         subcontent = fileInfo.SubContentToUpload,
                         moviefps = string.Empty,
                         movieframes = string.Empty,
                         movietimems = string.Empty
                     }));

                
                finalUrl = RegexHelper.GetUploadUrl(post);
                
            }
            else
            {
                throw new Exception("Duplicate file, alreadyindb");
            }

            progress(100);
            return finalUrl.Trim();
        }
        // Private Methods (1) 

        void tryLogin()
        {
            //if (!string.IsNullOrEmpty(_loginToken)) return;

            //var loginInfo = _client.LogIn(string.Empty, string.Empty, string.Empty, UA.MyUA);
            //var status = loginInfo.status;
            //if (string.IsNullOrWhiteSpace(status) || status != "200 OK")
            //{
            //    throw new Exception("Couldn't login.");
            //}
            _loginToken = LoginTokenHolder.Instance.LoginToken;
        }

        #endregion Methods
    }
}
