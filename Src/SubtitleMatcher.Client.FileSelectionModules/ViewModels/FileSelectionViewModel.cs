using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Win32;
using SubtitleMatcher.Client.Common;
using Microsoft.Practices.Composite.Events;
using SubtitlesMatcher.Common.Parser;
using Microsoft.Practices.Composite.Presentation.Events;

namespace SubtitleMatcher.Client.FileSelectionModules.ViewModels
{
    public class FileSelectionViewModel : IFileSelectionViewModel,INotifyPropertyChanged
    {
        private IEventAggregator _eventAggregator;
        public DelegateCommand<object> BrowseDelegateCommand { get; set; }

        public FileSelectionViewModel(IEventAggregator eventAggregator)
        {
            BrowseDelegateCommand = new DelegateCommand<object>(BrowseFile);
            _eventAggregator = eventAggregator;
      
        }

        private void BrowseFile(object obj)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "AVI Files (.avi)|*.avi;*.mkv|All Files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.Multiselect = true;
            bool? res = fileDialog.ShowDialog();
            if(res.HasValue && res.Value) {
                FileNames = new List<string>( fileDialog.FileNames);
                PublishEvent();
            }
        }

        private List<string> _fileNames;

        public List<string> FileNames
        {
            get { return _fileNames; }
            set
            {
                _fileNames = value;
                OnPropertyChanged("FileNames");
            }
        }

        private MediaFileInfo _mediaFileInfo;

        public MediaFileInfo MediaFileInfo
        {
            get { return _mediaFileInfo; }
            set
            {
                if (_mediaFileInfo != null)
                {
                    _mediaFileInfo.PropertyChanged -= MediaFileInfo_PropertyChanged;
                }
                _mediaFileInfo = value;
                if (_mediaFileInfo != null)
                {
                    _mediaFileInfo.PropertyChanged += MediaFileInfo_PropertyChanged;
                }
                OnPropertyChanged("MediaFileInfo");
            }
        }

        void MediaFileInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PublishEvent();
        }

        private bool _isTvShow;

        public bool IsTvShow
        {
            get { return _isTvShow; }
            set
            {
                if (_isTvShow != value)
                {
                    if (value)
                    {
                        MediaFileInfo = new TvShowMediaFileInfo();
                    }
                    else
                    {
                        MediaFileInfo = new MediaFileInfo();
                    }
                }
                _isTvShow = value;
              
                OnPropertyChanged("IsTvShow");
            }
        }

        private bool _overrideFileName;

        public bool OverrideFileName
        {
            get { return _overrideFileName; }
            set
            {
                _overrideFileName = value;
                if (_overrideFileName == false)
                {
                    MediaFileInfo = null;
                }
                else
                {
                    if (IsTvShow)
                    {
                        MediaFileInfo = new TvShowMediaFileInfo();
                    }
                    else
                    {
                        MediaFileInfo = new MediaFileInfo();
                    }
                }
                OnPropertyChanged("OverrideFileName");
                PublishEvent();
            }
        }

        private void PublishEvent()
        {
            _eventAggregator.GetEvent<CompositePresentationEvent<TargetSelectionEvent>>().Publish(new TargetSelectionEvent()
            {
                FileNames = FileNames,
                MediaFileInfo = MediaFileInfo
            });
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
