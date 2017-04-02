using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SubtitlesMatcher.Common.Parser
{
    public class TvShowMediaFileInfo : MediaFileInfo, INotifyPropertyChanged
    {
        private int _season;

        public int Season
        {
            get { return _season; }
            set
            {
                _season = value;
                OnPropertyChanged("Season");
            }
        }
        private int _episode;

        public int Episode
        {
            get { return _episode; }
            set
            {
                _episode = value;
                OnPropertyChanged("Episode");
            }
        }
    }
}
