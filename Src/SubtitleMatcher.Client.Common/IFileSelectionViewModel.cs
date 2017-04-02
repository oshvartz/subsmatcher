using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleMatcher.Client.Common
{
    public interface IFileSelectionViewModel
    {
        List<string> FileNames { get; }
    }
}
