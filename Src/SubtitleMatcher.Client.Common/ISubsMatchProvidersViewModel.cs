using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubtitlesMatcher.Common;
using System.Globalization;

namespace SubtitleMatcher.Client.Common
{
    public interface ISubsMatchProvidersViewModel
    {
        ISubtitleMatcherProvider TheSelectedSubtitleMatcherProvider { get; }
        CultureInfo SelectedLanguage { get; }
    }
}
