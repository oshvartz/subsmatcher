using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using SubtitleMatcher.Client.Common;
using SubtitlesMatcher.Common;
using System.Globalization;
using SubtitlesMatcher.Common.Interfaces;
using System.Threading.Tasks;
using System.ComponentModel;
using SubtitlesMatcher.Common.Parser;
using System.IO;
using System.Threading;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using System.Windows;
using SubtitlesMatcher.Common.Events;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using SubtitleMatcher.Client.ActionsModules.Views;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace SubtitleMatcher.Client.ActionsModules.ViewModels
{
    public class ActionsViewModel : INotifyPropertyChanged
    {
        private IUnityContainer _container;
        private const int TIMEOUT = 60 * 1000 * 20;
        public DelegateCommand<object> FindSubsCommand { get; private set; }
        public ActionsViewModel(IUnityContainer container, IEventAggregator eventAggregator)
        {
            _container = container;
            FindSubsCommand = new DelegateCommand<object>(FindSubs);
            ISubtitlesMatcherMgr subtitlesMatcherMgr = _container.Resolve<ISubtitlesMatcherMgr>();
            subtitlesMatcherMgr.OnSubsMatcherStatusChanged += new SubsRetrieveStatusEventHandler(subtitlesMatcherMgr_OnSubsMatcherStatusChanged);
            subtitlesMatcherMgr.OnMultiSubsMatch += subtitlesMatcherMgr_OnMultiSubsMatch;
            FindEnable = true;
            eventAggregator.GetEvent<CompositePresentationEvent<TargetSelectionEvent>>().Subscribe(
                targetSelectionEvent =>
                {
                    FileNames = targetSelectionEvent.FileNames;
                    MediaFileInfo = targetSelectionEvent.MediaFileInfo;
                }, ThreadOption.UIThread
                );

            SearchSubsStatuses = new ObservableCollection<string>();
            _OverrideByFileName = true;
        }

        void subtitlesMatcherMgr_OnMultiSubsMatch(object sender, MultiSubsMatchEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action( () =>
            {
                MultiMatchWindow multiMatchWindow = new MultiMatchWindow();
                multiMatchWindow.DataContext = e;
                multiMatchWindow.Owner = Application.Current.MainWindow;
                multiMatchWindow.ShowDialog();
                
            }));
        }

        public ObservableCollection<string> SearchSubsStatuses { get; set; }

        private bool _silentMode;

        public bool SilentMode
        {
            get { return _silentMode; }
            set
            {
                _silentMode = value;
                OnPropertyChanged("SilentMode");
            }
        }

        private bool _OverrideByFileName;

        public bool OverrideByFileName
        {
            get { return _OverrideByFileName; }
            set
            {
                _OverrideByFileName = value;
                OnPropertyChanged("OverrideByFileName");
            }
        }

        

        private bool _inProgress;

        public bool InProgress
        {
            get { return _inProgress; }
            set
            {
                _inProgress = value;
                OnPropertyChanged("InProgress");
            }
        }

        private bool _findEnable;

        public bool FindEnable
        {
            get { return _findEnable; }
            set
            {
                _findEnable = value;
                OnPropertyChanged("FindEnable");
            }
        }


        private string _searchSubsStatus;

      

        void subtitlesMatcherMgr_OnSubsMatcherStatusChanged(SubtitlesMatcher.Common.Events.SubtitlesMatcherEventArgs subtitlesMatcherEventArgs)
        {
            string msg = subtitlesMatcherEventArgs.Message == string.Empty ? string.Empty : Path.GetFileName(subtitlesMatcherEventArgs.Message);
      

            Application.Current.Dispatcher.Invoke(new Action( () =>
            SearchSubsStatuses.Add(string.Format("{0}|{1}", msg, subtitlesMatcherEventArgs.Status.ToString()))));
        }

        public List<string> FileNames { get; set; }
        public MediaFileInfo MediaFileInfo { get; set; }

        private void FindSubs(object obj)
        {
            ISubsMatchProvidersViewModel subsMatchProvidersViewModel = _container.Resolve<ISubsMatchProvidersViewModel>();

            if (subsMatchProvidersViewModel != null && subsMatchProvidersViewModel.TheSelectedSubtitleMatcherProvider == null)
            {
                MessageBox.Show("You must select search provider first");
                return;
            }

            if (subsMatchProvidersViewModel != null && subsMatchProvidersViewModel.SelectedLanguage != null && subsMatchProvidersViewModel.TheSelectedSubtitleMatcherProvider != null &&
                FileNames != null && FileNames.Count > 0)
            {
                if (MediaFileInfo != null && FileNames != null && FileNames.Count > 1)
                {
                    MessageBox.Show("You cannot choose more then one file in case of override media info");
                    return;
                }
                FindSubsAsync();
            }

        }

        private CancellationToken _token = CancellationToken.None;

        private void FindSubsAsync()
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                _token = cts.Token;

                FindEnable = false;
                InProgress = true;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                            SearchSubsStatuses.Clear()));

                Task.Factory.StartNew(FindSubs, _token).ContinueWith(t =>
                {
                    FindEnable = true;
                    InProgress = false;
                    _token = CancellationToken.None;
                });
            }
            catch
            {
                FindEnable = true;
                InProgress = false;
                throw;
            }
            
        }

        private void FindSubs()
        {
            try
            {
                ISubsMatchProvidersViewModel subsMatchProvidersViewModel = _container.Resolve<ISubsMatchProvidersViewModel>();

                ISubtitlesMatcherMgr subtitlesMatcherMgr = _container.Resolve<ISubtitlesMatcherMgr>();
                IMediaFileNameParser mediaFileNameParser = _container.Resolve<IMediaFileNameParser>();
                if (MediaFileInfo != null)
                {
                    subtitlesMatcherMgr.FindAndExtractSubtitles(mediaFileNameParser, subsMatchProvidersViewModel.TheSelectedSubtitleMatcherProvider, FileNames[0], subsMatchProvidersViewModel.SelectedLanguage, MediaFileInfo,SilentMode,OverrideByFileName);
                }
                else
                {
                    subtitlesMatcherMgr.FindAndExtractSubtitles(mediaFileNameParser, subsMatchProvidersViewModel.TheSelectedSubtitleMatcherProvider, FileNames, subsMatchProvidersViewModel.SelectedLanguage, _token, SilentMode, OverrideByFileName);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error: see event viewer for details");
                EventLog.WriteEntry("Application", ex.ToString(), EventLogEntryType.Error);

            }

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
