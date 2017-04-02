using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using SubtitlesMatcher.Common;
using SubtitlesMatcher.Server;
using System.Configuration;
using System.ComponentModel.Composition.Hosting;
using SubCenterSubtitlesMatcher.Parser;
using System.Globalization;
using System.IO;
using SubtitlesMatcher.Common.Parser;

namespace SubtitlesMatcher.SilentRunner
{
    public class SubsSilentFinder
    {
        [ImportMany(typeof(ISubtitleMatcherProvider))]
        private List<ISubtitleMatcherProvider> _importedSubtitleMatcherProvider;
        private SubtitlesMatcherMgr _manager;

        public SubsSilentFinder()
        {
            _manager = new SubtitlesMatcherMgr();
            _manager.OnSubsMatcherStatusChanged += OnSubsMatcherStatusChanged;
        }

        void OnSubsMatcherStatusChanged(SubtitlesMatcher.Common.Events.SubtitlesMatcherEventArgs e)
        {
            string file = Path.GetFileName(e.Message);
            Console.WriteLine(e.Status.ToString() + " " + file + '\n');
        }

        public void FindAndDownloadSub(string path)
        {

            string providerName = ConfigurationManager.AppSettings.Get("Provider");
            string providerDllPath = ConfigurationManager.AppSettings.Get("ProvidersPath");
            string culture = ConfigurationManager.AppSettings.Get("Culture");
            string searchPattern = ConfigurationManager.AppSettings.Get("SearchPatterns");
            string[] searchPatterns = searchPattern.Split(";".ToCharArray());

            if (string.IsNullOrEmpty(providerDllPath))
            {
                providerDllPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            }


            //load providers
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(providerDllPath));
            var container = new CompositionContainer(catalog);

            container.SatisfyImportsOnce(this);

            ISubtitleMatcherProvider matchProvider = (from provider in _importedSubtitleMatcherProvider where provider.ProviderName == providerName select provider).FirstOrDefault();

            if (matchProvider != null)
            {
                //this is a folder
                List<string> fileNames = GetAllMediaFiles(path, searchPatterns);

                foreach (string fileName in fileNames)
                {
                    MatchFile(fileName, culture, matchProvider);
                }
            }

            Console.WriteLine("Press enter to continue...");
            Console.Read();
        }

        private void MatchFile(string fileName, string culture, ISubtitleMatcherProvider matchProvider)
        {
            Console.Write("Matching: " + fileName + '\n');
            IMediaFileNameParser parser = new MediaFileNameParser();
            MediaFileInfo info = parser.Parse(fileName, matchProvider.SearchIsHashBase);

            _manager.FindAndExtractSubtitles(parser, matchProvider, fileName, new CultureInfo(culture), info, true, true);
        }

        private List<string> GetAllMediaFiles(string path, string[] searchPatterns)
        {
            List<string> files = new List<string>();
            if (File.Exists(path))
            {
                //this is a file
                files.Add(path);
                return files;
            }

            GetAllMediaFiles(path, searchPatterns, files);
            return files;
        }

        private void GetAllMediaFiles(string path, string[] searchPatterns, List<string> mediaFiles)
        {
            foreach (string searchPattern in searchPatterns)
            {
                foreach (string file in Directory.GetFiles(path, searchPattern))
                {
                    mediaFiles.Add(file);
                }
            }
            foreach (string subPath in Directory.GetDirectories(path))
            {
                GetAllMediaFiles(subPath, searchPatterns, mediaFiles);
            }
        }


    }
}
