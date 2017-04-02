using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SubtitlesMatcher.Common.Results
{
    public class SubtitleMatch
    {
        public CultureInfo Language { get; set; }
        public string SubFileName { get; set; }
        public string SubFileUrl { get; set; }

    }
}
