using System;
namespace SubtitlesMatcher.Common.Parser
{
    public interface IMediaFileNameParser
    {
        MediaFileInfo Parse(string mediaFileName, bool searchIsHashBase);
    }
}
