using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubtitlesMatcher.Common.Results;
using SubtitlesMatcher.Infrastructure;
using System.Text.RegularExpressions;
using System.Globalization;
using SubtitlesMatcher.Common.Parser;

namespace SubCenterSubtitlesMatcher.SubtiltleFileSearcher
{
    public class SubCenterSubtitleSearcher : SubCenterSubtitlesMatcher.SubtiltleFileSearcher.ISubtitleSearcher
    {

        private const string BASE_URL = "http://www.subscenter.org/index.php";
        private static readonly string _searchUrlPattern = "?page=search&searchQuery={0}&type={1}&order=u";
        private static readonly string _episodIdUrlPattern = "?page=episode&episodeId={0}";
        private static readonly string _searchEpisodePattern = "episodeId=(\\d+)\"><strong>פרק מספר (\\d+)";
        private Encoding _encoding;

        public SubCenterSubtitleSearcher(Encoding encoding)
        {
            _encoding = encoding;
        }
        

        private static readonly string _searchResPattern = "<a href=\"(http://www\\.subscenter.org/index\\.php\\?page=view&viewId=\\d+)\" class=\"footerlink\">\n.+{0}";

        public List<SubtitleMatch> Find(MediaFileInfo mediaFileInfo)
        {
            string resposeUrl;
            int mediatype = GetMediaType(mediaFileInfo);
            string tragetUrl = BASE_URL + string.Format(_searchUrlPattern, mediaFileInfo.TitleName, mediatype);
            string searchUrlContent = WebPageHelper.RetrieveWebPageContent(tragetUrl, _encoding,out resposeUrl);

            //the search retured more then one result
            if (resposeUrl == tragetUrl)
            {
                searchUrlContent = GetSelectedSeachUrlContent(searchUrlContent, mediaFileInfo.TitleName, tragetUrl);
            }

            List<SubtitleMatch> res = null;

            if (mediaFileInfo is TvShowMediaFileInfo)
            {
                res = Find((TvShowMediaFileInfo)mediaFileInfo, searchUrlContent);
            }
            else
            {
                res = Find(mediaFileInfo, searchUrlContent);
            }


            return res;
        }

        private static int GetMediaType(MediaFileInfo mediaFileInfo)
        {
            //movie
            int mediatype = 1;
            if (mediaFileInfo is TvShowMediaFileInfo)
            {
                mediatype = 2;
            }
            return mediatype;
        }

        private static readonly string _subTitleLang = "img src=\"images/flag_(\\w+)";
        private static readonly string _subTitleVersion = "subtitle_versionTD\">([ \\s\\[\\]\\)\\(\\w\\d\\.\\-\\–]+)</td>";
        private static readonly string _downloadSubId = "download\\.php\\?subId=\\d+";

        private List<SubtitleMatch> Find(MediaFileInfo mediaFileInfo, string searchUrlContent)
        {
            List<SubtitleMatch> res = new List<SubtitleMatch>();

            var langMatchCollection = Regex.Matches(searchUrlContent, _subTitleLang);
            var verMatchCollection = Regex.Matches(searchUrlContent, _subTitleVersion);
            var downloadUrlSuffixCollection = Regex.Matches(searchUrlContent, _downloadSubId);

            for (int i = 0; i < langMatchCollection.Count; i++)
            {
                bool isHebrew = langMatchCollection[i].Groups[1].Value == "israel";

                bool isVersionMatch = -1 != verMatchCollection[i].Groups[1].Value.IndexOf(mediaFileInfo.VersionName, StringComparison.OrdinalIgnoreCase);

                if (isHebrew && isVersionMatch)
                {
                    res.Add(new SubtitleMatch
                    {
                        Language =   new CultureInfo("he-IL"),
                        SubFileName = verMatchCollection[i].Groups[1].Value,
                        SubFileUrl = string.Format("http://www.subscenter.org/{0}", downloadUrlSuffixCollection[i].Groups[0].Value),
                    }
                    );

                }

            }

            return res;
        }

        private List<SubtitleMatch> Find(TvShowMediaFileInfo tvShowMediaFileInfo, string searchUrlContent)
        {
            if (searchUrlContent == string.Empty)
            {
                return new List<SubtitleMatch>();
            }
            Dictionary<int, Dictionary<int, string>> seasonEpsEpisodeIdDic = new Dictionary<int, Dictionary<int, string>>();
            string newSearchUrlContent = null;
            var matchCollection = Regex.Matches(searchUrlContent, _searchEpisodePattern);
            int season = 0;
            foreach (Match match in matchCollection)
            {
                int epNum = int.Parse(match.Groups[2].Value);
                if (epNum == 1)
                {
                    season++;
                    seasonEpsEpisodeIdDic.Add(season, new Dictionary<int, string>());
                }

                seasonEpsEpisodeIdDic[season].Add(epNum, match.Groups[1].Value);
                
            }

            if (seasonEpsEpisodeIdDic.Count == 0)
            {
                return new List<SubtitleMatch>();
            }

            int maxSeason = seasonEpsEpisodeIdDic.Keys.Max();
            
            int targetSeason = (maxSeason - tvShowMediaFileInfo.Season + 1) % (maxSeason + 1);
            if (seasonEpsEpisodeIdDic.ContainsKey(targetSeason))
            {
                if (seasonEpsEpisodeIdDic[targetSeason].ContainsKey(tvShowMediaFileInfo.Episode))
                {
                    int episodeId = int.Parse(seasonEpsEpisodeIdDic[targetSeason][tvShowMediaFileInfo.Episode]);
                    string tragetUrl = BASE_URL + string.Format(_episodIdUrlPattern, episodeId);
                    newSearchUrlContent = WebPageHelper.RetrieveWebPageContent(tragetUrl, _encoding);

                }

            }



            if (newSearchUrlContent != null)
            {

                return Find((MediaFileInfo)tvShowMediaFileInfo, newSearchUrlContent); ;
            }
            else
            {
                return new List<SubtitleMatch>();
            }
        }

        private static readonly string _findTitleRegex = "_thumb.jpg\" alt=\"([\\w\\d\\s\\.-]+)";
        private static readonly string _findUrlRegex = "navigate\\('\\?page=view\\&amp\\;viewId=(\\d+)";
        private static readonly string _specifUrlRegex = "http://www.subscenter.org/index.php?page=view&viewId={0}";

        private string GetSelectedSeachUrlContent(string searchUrlContent, string title, string sourceUrl)
        {
        
            int currPage = 0;

            while (!searchUrlContent.Contains("לא נמצאו תוצאות"))
            {

                MatchCollection titlesMatches = Regex.Matches(searchUrlContent, _findTitleRegex);
                MatchCollection urlsMatches = Regex.Matches(searchUrlContent, _findUrlRegex);

                for (int i = 0; i < titlesMatches.Count; i++)
                {
                    if (titlesMatches[i].Groups[1].Value.Contains( title + " "))
                    {
                        string tragetUrl = string.Format( _specifUrlRegex ,urlsMatches[i].Groups[1].Value);
                        
                        return WebPageHelper.RetrieveWebPageContent(tragetUrl,_encoding);

                    }
                }

            
                currPage++;
                string newSourceUrl = string.Format(sourceUrl + "&p={0}", currPage);
                searchUrlContent = WebPageHelper.RetrieveWebPageContent(newSourceUrl, _encoding);

            }

            return string.Empty;

        }
    }
}
