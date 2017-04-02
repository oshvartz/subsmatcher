using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubtitlesMatcher.Common.Interfaces;
using SubtitlesMatcher.Common;
using System.Globalization;
using SubtitlesMatcher.Infrastructure;
using System.IO;
using SubtitlesMatcher.Common.Events;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using SubtitlesMatcher.Common.Parser;
using System.Threading;
using SubtitlesMatcher.Common.Results;

namespace SubtitlesMatcher.Server
{
    public class SubtitlesMatcherMgr : ISubtitlesMatcherMgr
    {
        private string _tempFolderPath;
        
        public SubtitlesMatcherMgr()
        {
        
            _tempFolderPath = Path.Combine(Environment.CurrentDirectory, "..\\Temp");
            if (!Directory.Exists(_tempFolderPath))
            {
                Directory.CreateDirectory(_tempFolderPath);
            }


        }

        #region ISubtitlesMatcherMgr Members




        public void FindAndExtractSubtitles(IMediaFileNameParser mediaFileNameParser, ISubtitleMatcherProvider subtitleMatcherProvider, List<string> mediaFileNames, CultureInfo language, CancellationToken token, bool silentMode = false, bool ovrrideByFileName = false)
        {
            if (token != null && token != CancellationToken.None)
            {
                token.ThrowIfCancellationRequested();
            }
            mediaFileNames.ForEach(mediaFileName => {

                FindAndExtractSubtitles(mediaFileNameParser, subtitleMatcherProvider, mediaFileName, language, null, silentMode, ovrrideByFileName);
            });
        }


        public void FindAndExtractSubtitles(IMediaFileNameParser mediaFileNameParser, ISubtitleMatcherProvider subtitleMatcherProvider, string mediaFileName, CultureInfo language, MediaFileInfo mediaFileInfo, bool silentMode = false, bool ovrrideByFileName = false)
        {
            
            if (mediaFileInfo == null)
            {
                FireOnSubsMatcherStatusChanged(mediaFileName, EnumStatus.Parsing);
                mediaFileInfo = mediaFileNameParser.Parse(mediaFileName, subtitleMatcherProvider.SearchIsHashBase);
            }
            else
            {
                mediaFileInfo.FileName = mediaFileName;
            }
            var subsMatchs = subtitleMatcherProvider.Find(mediaFileInfo, language);

            

            if (subsMatchs.Count > 1 && !silentMode)
            {

                var multiSubsMatchEventArgs =  new MultiSubsMatchEventArgs() { SubtitleMatchs = subsMatchs };
                if (!ovrrideByFileName || !TryFindMatchByStrings(multiSubsMatchEventArgs, mediaFileName))
                {
                    OnMultiSubsMatch(this, multiSubsMatchEventArgs);
                }
              
                if (multiSubsMatchEventArgs.SelectedMatchIndex != -1)
                {
                    DownloadAndRename(subsMatchs[multiSubsMatchEventArgs.SelectedMatchIndex].SubFileUrl, mediaFileName);
                    FireOnSubsMatcherStatusChanged(mediaFileName, EnumStatus.Done);
                    return;
                }
                else
                {
                    FireOnSubsMatcherStatusChanged(mediaFileName, EnumStatus.SubtitlesNotFound);
                }

            }
            else if (subsMatchs.Count > 0)
            {
                FireOnSubsMatcherStatusChanged(subsMatchs[0].SubFileName, EnumStatus.SubtitlesFound);
                DownloadAndRename(subsMatchs[0].SubFileUrl, mediaFileName);
                FireOnSubsMatcherStatusChanged(mediaFileName, EnumStatus.Done);
            }
            else
            {
                //retry with no version
                if (mediaFileInfo.VersionName != string.Empty && !subtitleMatcherProvider.SearchIsHashBase)
                {
                    mediaFileInfo.VersionName = string.Empty;
                    FindAndExtractSubtitles(mediaFileNameParser, subtitleMatcherProvider, mediaFileName, language, mediaFileInfo);
                }
                else
                {
                    FireOnSubsMatcherStatusChanged(mediaFileName, EnumStatus.SubtitlesNotFound);
                }
            }
        }

        private bool TryFindMatchByStrings(MultiSubsMatchEventArgs multiSubsMatchEventArgs, string mediaFileName)
        {
            multiSubsMatchEventArgs.SelectedMatchIndex = -1;
            if (string.IsNullOrEmpty(mediaFileName))
            {
                return false;
            }

            var foundSubsList = multiSubsMatchEventArgs.SubtitleMatchs.Select(sm => sm.SubFileName.Replace(".srt", string.Empty)).ToList();

            multiSubsMatchEventArgs.SelectedMatchIndex = foundSubsList.FindIndex(subName => mediaFileName.IndexOf(subName) != -1);
            return multiSubsMatchEventArgs.SelectedMatchIndex != -1;
        }

        private void DownloadAndRename(string downloadUrl, string mediaFileName)
        {
            string tempFilePath = Path.Combine(_tempFolderPath, Guid.NewGuid().ToString());

            var subFileName = Path.ChangeExtension(mediaFileName, "srt");

            FireOnSubsMatcherStatusChanged(subFileName, EnumStatus.Downloading);
            FileDownloader.Download(downloadUrl, tempFilePath);

            if (downloadUrl.EndsWith(".gz"))
            {
                FireOnSubsMatcherStatusChanged(subFileName, EnumStatus.Extracting);
                ZipExtractor.ExtractGz(tempFilePath, tempFilePath);
                RenameFile(mediaFileName, tempFilePath);
            }

            else if (ZipExtractor.IsZipFile(tempFilePath))
            {
                FireOnSubsMatcherStatusChanged(subFileName, EnumStatus.Extracting);
                List<string> srtFiles = ZipExtractor.ExtractFiles(tempFilePath,"srt",_tempFolderPath);

                if (srtFiles.Count == 1)
                {
                    RenameFile(mediaFileName, Path.Combine(_tempFolderPath,srtFiles[0]));
                }
                else
                {
                    throw new ApplicationException("The zip doesn't contains srt files");
                }

            }
            else
            {
                RenameFile(mediaFileName, tempFilePath);
            }

            File.Delete(tempFilePath);

        }

        private static void RenameFile(string mediaFileName,string srtFile)
        {
            FileInfo fi = new FileInfo(srtFile);
            string targetFilePath = Path.Combine(Path.GetDirectoryName(mediaFileName), Path.GetFileNameWithoutExtension(mediaFileName) + ".srt");
            if (File.Exists(targetFilePath))
            {
                File.Delete(targetFilePath);
            }
            fi.MoveTo(targetFilePath);
        }
        

       

        public event SubsRetrieveStatusEventHandler OnSubsMatcherStatusChanged = delegate { };

        public event EventHandler<MultiSubsMatchEventArgs> OnMultiSubsMatch = delegate { };

        private void FireOnSubsMatcherStatusChanged(string messege, EnumStatus staus)
        {
            OnSubsMatcherStatusChanged(new SubtitlesMatcherEventArgs() { Message = messege, Status = staus });
        }


        #endregion

   
    }
}
