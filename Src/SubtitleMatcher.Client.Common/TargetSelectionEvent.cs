using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using SubtitlesMatcher.Common.Parser;

namespace SubtitleMatcher.Client.Common
{
    public class TargetSelectionEvent 
    {

        public List<string> FileNames { get; set; }
        public MediaFileInfo MediaFileInfo { get; set; }

    
    }
}
