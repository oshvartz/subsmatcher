using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubtitlesMatcher.Common;
using System.Globalization;
using SubtitlesMatcher.Common.Results;
using SubCenterSubtitlesMatcher.SubtiltleFileSearcher;
using SubtitlesMatcher.Infrastructure;
using Microsoft.Practices.Unity;
using System.ComponentModel.Composition;
using SubtitlesMatcher.Common.Parser;

namespace SubCenterSubtitlesMatcher
{
    [Export(typeof(ISubtitleMatcherProvider))]
    public class SubCenterSubtitleMatcherProvider : ISubtitleMatcherProvider
    {
        
        public SubCenterSubtitleMatcherProvider()
        {
            IoC.Instance.RegisterInstance<ISubtitleSearcher>(new SubCenterSubtitleSearcher(Encoding));
        }

        
        #region ISubtitleMatcherProvider Members

        public bool SearchIsHashBase { get { return false; } }

        public List<CultureInfo> SupportedLanguages
        {
            get
            {
                return new List<CultureInfo>
                    {
                        new CultureInfo("he-IL")
                    };
            }
        }

        public Encoding Encoding { get { return Encoding.GetEncoding(1255); } }


        public string ProviderName { get { return "Subscenter.org"; } }


        public List<SubtitleMatch> Find(MediaFileInfo mediaFileInfo, CultureInfo language)
        {
            if (!SupportedLanguages.Contains(language))
            {
                throw new ArgumentException("language is not supported:" + language.ToString());
            }

            ISubtitleSearcher subtitleSearcher = IoC.Instance.Resolve<ISubtitleSearcher>();

            return subtitleSearcher.Find(mediaFileInfo);
            
        }

        #endregion
    }
}
