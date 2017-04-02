using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using SubtitlesMatcher.Common.Parser;

namespace SubCenterSubtitlesMatcher.Parser
{
    public class MediaFileNameParser : IMediaFileNameParser
    {
        private static readonly List<string> _versionKeyWords = new List<string>
        {
           "xvid",
           "hdtv",
           "dvdrip",
           "R5",
           "720p",
           "BDRip",
           "HD",
           "proper"

        };

        private const string TVSHOW_REGEX_PATT = @"S\d+E\d+";
        private const string TVSHOW_REGEX_PATT_OP2 = @"\d+x\d+";
        private const string TVSHOW_REGEX_PATT_OP3 = @"\d\dx\d+";
        private const string YEAR_REG_PATT = @"[\(\[\.-]\d\d\d\d";
        
        public MediaFileNameParser()
        {

        }

        public MediaFileInfo Parse(string mediaFileName, bool searchIsHashBase)
        {
            var origMediaFileName = mediaFileName;
            mediaFileName = Path.GetFileNameWithoutExtension(mediaFileName);

            MediaFileInfo res = null;

            if (searchIsHashBase)
            {
                res = new MediaFileInfo() { TitleName = "N/A", VersionName = "N/A" };
            }
            else
            {

                if (IsTvShow(mediaFileName))
                {
                    res = ParseTvShow(mediaFileName);
                }
                else
                {
                    res = ParseMovie(mediaFileName);
                }
            }

            res.FileName = origMediaFileName;
            return res;
        }

        private MediaFileInfo ParseMovie(string mediaFileName)
        {
            int sepIndex = FindMediaFileNameTitleAndVersionSepertionIndex(mediaFileName);
            string rawTile,rawVersion;
            if (sepIndex == int.MaxValue || sepIndex == 0)
            {
                rawTile = mediaFileName;
                rawVersion = string.Empty;

            }
            else
            {
                rawTile = mediaFileName.Substring(0, sepIndex);
                rawVersion = mediaFileName.Substring(sepIndex);
            }
            string title = GetTitle(rawTile);
            string version = GetVersion(rawVersion);

            return new MediaFileInfo
            {
                TitleName = title,
                VersionName = version
            };

        }
        
        private int FindMediaFileNameTitleAndVersionSepertionIndex(string mediaFileName)
        {
            int resIndex = int.MaxValue;

            foreach (var keyword in _versionKeyWords)
            {
                int index = mediaFileName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                {
                    resIndex = index < resIndex ? index : resIndex;
                }
            }

            Match match = Regex.Match(mediaFileName, YEAR_REG_PATT);
            if (match.Success)
            {
                int index = mediaFileName.IndexOf(match.Groups[0].Value);
                resIndex = index < resIndex ? index : resIndex;
            }

            return resIndex;
        }

        private MediaFileInfo ParseTvShow(string mediaFileName)
        {
            mediaFileName = Regex.Replace(mediaFileName, YEAR_REG_PATT, string.Empty);
            Regex tvShowRegex = new Regex(@"([\w\d\.\s-]+)(" + TVSHOW_REGEX_PATT + @")([\w\d\.\s-]*)", RegexOptions.IgnoreCase);
            Regex tvShowRegexOp3 = new Regex(@"([\w\d\.\s-]+)(" + TVSHOW_REGEX_PATT_OP3 + @")([\w\d\.\s-]*)", RegexOptions.IgnoreCase);
            Regex tvShowRegexOp2 = new Regex(@"([\w\d\.\s-]+)(" + TVSHOW_REGEX_PATT_OP2 + @")([\w\d\.\s-]*)", RegexOptions.IgnoreCase);

            Match match = tvShowRegex.Match(mediaFileName);

            if (!match.Success)
            {
                match = tvShowRegexOp3.Match(mediaFileName);
            }

            if (!match.Success)
            {
                match = tvShowRegexOp2.Match(mediaFileName);
            }

            if (!match.Success)
            {
                throw new ArgumentException("Illegal media file format:" + mediaFileName);
            }

            string title = GetTitle(match.Groups[1].Value);
            int season,episode;
            GetSeasonAndEpisode(match.Groups[2].Value, out season,out episode);
            string version = GetVersion(match.Groups[3].Value);

            return new TvShowMediaFileInfo()
            {
             TitleName = title,
             VersionName = version,
             Episode =episode,
             Season = season
           
            };
            
        }

      

        private string GetVersion(string versionRawString)
        {
            string res = versionRawString;

            Match verMatch = Regex.Match(res, @"[-\.]([\w\d]+)$", RegexOptions.RightToLeft);

            if (verMatch.Success)
            {
                res = verMatch.Groups[1].Value;
            }
            else
            {

                foreach (var key in _versionKeyWords)
                {
                    var matchKey = Regex.Match(res, @"[^\w\d](" + key + @")[^\w\d]", RegexOptions.IgnoreCase);
                    if (matchKey.Success)
                    {
                        res = res.Replace(matchKey.Groups[1].Value, string.Empty);
                    }
                }


                res = Regex.Replace(res, @"[\s\.-]+", string.Empty);
                res = Regex.Replace(res, @"[\[\(]?\d\d\d\d[\]\)]?", string.Empty);
            }
            
        
            return res;
        }

        private void GetSeasonAndEpisode(string seasonAndEpRawString, out int season, out int episode)
        {
            Regex findPattren = new Regex(@"S?(\d+)E?x?(\d+)", RegexOptions.IgnoreCase);
            Match match = findPattren.Match(seasonAndEpRawString);
            season = int.Parse(match.Groups[1].Value);
            episode = int.Parse(match.Groups[2].Value);
        }


        private string GetTitle(string titleRawString)
        {
           
            var res =  Regex.Replace(titleRawString, @"[\s\.-]+$", string.Empty);

            MatchCollection lowerUpperMatchColl = Regex.Matches(res, @"([a-z])([A-Z])");
            foreach (Match match in lowerUpperMatchColl)
            {
                res = res.Replace(match.Groups[0].Value, match.Groups[1].Value + " " + match.Groups[2].Value);
            }
            return res;

        }

        
        private bool IsTvShow(string mediaFileName)
        {
            return Regex.IsMatch(mediaFileName, TVSHOW_REGEX_PATT, RegexOptions.IgnoreCase) ||
            Regex.IsMatch(mediaFileName, TVSHOW_REGEX_PATT_OP2, RegexOptions.IgnoreCase);
        }



    }
}
