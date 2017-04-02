using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SubtitlesMatcher.Common.Results;

namespace SubtitlesMatcher.Common.Events
{
    public class MultiSubsMatchEventArgs : EventArgs, INotifyPropertyChanged
    {

        private int _selectedMatchIndex = -1;

        public int SelectedMatchIndex
        {
            get { return _selectedMatchIndex; }
            set
            {
                _selectedMatchIndex = value;
                OnPropertyChanged("SelectedMatchIndex");
            }
        }


        private List<SubtitleMatch> _subtitleMatchs;

        public List<SubtitleMatch> SubtitleMatchs
        {
            get { return _subtitleMatchs; }
            set
            {
                _subtitleMatchs = value;
                OnPropertyChanged("SubtitleMatchs");
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
