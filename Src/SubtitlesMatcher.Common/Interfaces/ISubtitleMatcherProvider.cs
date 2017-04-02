using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using SubtitlesMatcher.Common.Results;
using SubtitlesMatcher.Common.Parser;


namespace SubtitlesMatcher.Common
{
    public interface ISubtitleMatcherProvider
    {
        //The languages this provider supports
        List<CultureInfo> SupportedLanguages { get; }
        //execute search
        List<SubtitleMatch> Find(MediaFileInfo mediaFileInfo, CultureInfo language);
        //provider display name
        string ProviderName { get; }
        bool SearchIsHashBase { get; }
    }
}
