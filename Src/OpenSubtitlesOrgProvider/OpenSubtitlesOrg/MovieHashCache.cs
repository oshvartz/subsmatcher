using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API;

namespace OpenSubtitlesOrgMatcher.OpenSubtitlesOrg
{
    public class MovieHashCache
    {
        private static MovieHashCache _instance = new MovieHashCache();
        public static MovieHashCache Instance { get { return _instance; } }
        private MovieHashCache()
        {
            _cacheDic = new Dictionary<string, MovieCheckHashResult>();
        }

        private Dictionary<string, MovieCheckHashResult> _cacheDic;

        public MovieCheckHashResult GetMovieCheckHashResult(string movieHash)
        {
            MovieCheckHashResult res;
            if (_cacheDic.TryGetValue(movieHash, out res))
            {
                return res;
            }

            return null;
        }

        public void AddMovieCheckHashResultToCache(string movieHash, MovieCheckHashResult MovieCheckHashResult)
        {
            _cacheDic.Add(movieHash, MovieCheckHashResult);
        }
    }
}
