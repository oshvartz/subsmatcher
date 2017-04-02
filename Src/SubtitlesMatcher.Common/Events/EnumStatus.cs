using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitlesMatcher.Common.Events
{
    public enum EnumStatus
    {
        Parsing,
        SearchingWeb,
        SubtitlesFound,
        SubtitlesNotFound,
        Downloading,
        Extracting,
        Done
    }
}
