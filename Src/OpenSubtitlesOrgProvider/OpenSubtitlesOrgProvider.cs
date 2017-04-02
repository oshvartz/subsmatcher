using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubtitlesMatcher.Common;
using System.Globalization;
using SubtitlesMatcher.Common.Results;
using System.ComponentModel.Composition;
using SubtitlesMatcher.Common.Parser;
using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg;
using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API;

namespace TorecSubtitlesMatcher
{
    [Export(typeof(ISubtitleMatcherProvider))]
    public class OpenSubtitlesOrgProvider : ISubtitleMatcherProvider
    {
        #region ISubtitleMatcherProvider Members

        public bool SearchIsHashBase { get { return true; } }

        public List<CultureInfo> SupportedLanguages
        {
            get
            {
                return new List<CultureInfo>
                    {
                        new CultureInfo("he-IL"),
                        new CultureInfo("en"),
                        new CultureInfo("es"),
                        new CultureInfo("ja"),
                        new CultureInfo("pt"),
                        new CultureInfo("ru")
                    };
            }
        }

        public string ProviderName { get { return "OpenSubtitles.Org"; } }

        public Encoding Encoding { get { return Encoding.GetEncoding(1255); } }

        public List<SubtitleMatch> Find(MediaFileInfo mediaFileInfo, CultureInfo language)
        {
            string subLanguageId = ConvertLanguageToString(language);
            OpenSubtitlesXmlRpc openSubtitlesXmlRpc = new OpenSubtitlesXmlRpc(mediaFileInfo.FileName);

            var subRes = openSubtitlesXmlRpc.GetListOfAllSubtitles(prog => { }, subLanguageId);
            return ConvertSubtitlesSearchResultToSubtitleMatch(subRes,language);
        }

        private List<SubtitleMatch> ConvertSubtitlesSearchResultToSubtitleMatch(SubtitlesSearchResult subRes, CultureInfo language)
        {
            if (subRes == null || subRes.data == null || subRes.data.Length == 0)
            {
                return new List<SubtitleMatch>();
            }
            var res = new List<SubtitleMatch>(); 
            foreach (var subtitleDataInfo in subRes.data)
            {
                res.Add(GetSubtitleMatch(subtitleDataInfo, language)); 
            }
            return res;
        }

        private SubtitleMatch GetSubtitleMatch(SubtitleDataInfo subtitleDataInfo, CultureInfo language)
        {
            return new SubtitleMatch
            {
                Language = language,
                SubFileName = subtitleDataInfo.SubFileName,
                SubFileUrl = subtitleDataInfo.SubDownloadLink
            };
        }

        private string ConvertLanguageToString(CultureInfo language)
        {
            string res = string.Empty;

            switch (language.Name)
            {
                case "he-IL":
                    res = "heb";
                    break;

                case "en":
                    res = "eng";
                    break;

                case "es":
                    res = "spa";
                    break;
                case "ru":
                    res = "rus";
                    break;
                case "ja":
                    res = "jpn";
                    break;
                      case "pt":
                    res = "por";
                    break;


                
                default:
                    throw new ArgumentException("language is not supported:" + language.ToString());

            }

            return res;
        }


        #endregion
    }
}
