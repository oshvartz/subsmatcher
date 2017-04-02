using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitlesMatcher.Common.Events
{
    public class SubtitlesMatcherEventArgs : EventArgs
    {
        public EnumStatus Status { get; set; }
        public string Message { get; set; }
    }
}
