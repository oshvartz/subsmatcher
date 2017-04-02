using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using SubtitlesMatcher.Common.Events;
using SubtitlesMatcher.Common.Parser;
using System.Threading;

namespace SubtitlesMatcher.Common.Interfaces
{
    public delegate void SubsRetrieveStatusEventHandler(SubtitlesMatcherEventArgs subtitlesMatcherEventArgs);

    public interface ISubtitlesMatcherMgr
    {
        event SubsRetrieveStatusEventHandler OnSubsMatcherStatusChanged;

        event EventHandler<MultiSubsMatchEventArgs> OnMultiSubsMatch;

        void FindAndExtractSubtitles(IMediaFileNameParser mediaFileNameParser, ISubtitleMatcherProvider subtitleMatcherProvider, List<string> mediaFileNames, CultureInfo language, CancellationToken cancellationToken, bool silentMode = false, bool ovrrideByFileName = false);
        void FindAndExtractSubtitles(IMediaFileNameParser mediaFileNameParser, ISubtitleMatcherProvider subtitleMatcherProvider, string mediaFileName, CultureInfo language, MediaFileInfo mediaFileInfo, bool silentMode = false, bool ovrrideByFileName = false);
    }
}
