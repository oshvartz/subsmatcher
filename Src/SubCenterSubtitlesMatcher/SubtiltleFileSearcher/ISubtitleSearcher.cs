using System;
using System.Collections.Generic;
using SubtitlesMatcher.Common.Results;
using SubtitlesMatcher.Common.Parser;
namespace SubCenterSubtitlesMatcher.SubtiltleFileSearcher
{
    public interface ISubtitleSearcher
    {
        List<SubtitleMatch> Find(MediaFileInfo mediaFileInfo);
    }
}
