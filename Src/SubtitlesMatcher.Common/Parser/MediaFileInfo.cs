using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SubtitlesMatcher.Common.Parser
{
    public class MediaFileInfo : INotifyPropertyChanged
    {
        public MediaFileInfo()
        {
            TitleName = string.Empty;
            VersionName = string.Empty;
        }

        private string _titleName;

        public string TitleName
        {
            get { return _titleName; }
            set
            {
                _titleName = value;
                OnPropertyChanged("TitleName");
            }
        }
        private string _versionName;

        public string VersionName
        {
            get { return _versionName; }
            set
            {
                _versionName = value;
                OnPropertyChanged("VersionName");
            }
        }

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
