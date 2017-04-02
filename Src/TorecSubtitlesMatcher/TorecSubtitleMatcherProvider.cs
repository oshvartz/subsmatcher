using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubtitlesMatcher.Common;
using System.Globalization;
using SubtitlesMatcher.Common.Results;
using System.ComponentModel.Composition;
using SubtitlesMatcher.Common.Parser;

namespace TorecSubtitlesMatcher
{
    [Export(typeof(ISubtitleMatcherProvider))]
    public class TorecSubtitleMatcherProvider : ISubtitleMatcherProvider
    {
        #region ISubtitleMatcherProvider Members

        public bool SearchIsHashBase { get { return false; } }

        public List<CultureInfo> SupportedLanguages
        {
            get
            {
                return new List<CultureInfo>
                    {
                        new CultureInfo("he-IL"),
                        new CultureInfo("en")
                    };
            }
        }

        public string ProviderName { get { return "Other provider N/A"; } }

        public Encoding Encoding { get { return Encoding.GetEncoding(1255); } }

        public List<SubtitleMatch> Find(MediaFileInfo mediaFileInfo, CultureInfo language)
        {
            //TODO....
            return new List<SubtitleMatch>();
        }

        #endregion
    }
}
