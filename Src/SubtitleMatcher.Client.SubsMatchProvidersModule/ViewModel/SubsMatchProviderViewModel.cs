using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SubtitlesMatcher.Common;
using System.Globalization;

namespace SubtitleMatcher.Client.SubsMatchProvidersModule.ViewModel
{
    public class SubsMatchProviderViewModel : INotifyPropertyChanged
    {
        private ISubtitleMatcherProvider _subtitleMatcherProvider;

        public ISubtitleMatcherProvider SubtitleMatcherProvider
        {
            get
            {
                return _subtitleMatcherProvider;
            }
            set
            {
                _subtitleMatcherProvider = value;
                SelectedLanguage = _subtitleMatcherProvider.SupportedLanguages[0];
            }
        }

   

        #region ISubtitleMatcherProvider Members

        public List<CultureInfo> SupportedLanguages
        {
            get { return SubtitleMatcherProvider.SupportedLanguages; }
        }

        
        public string ProviderName
        {
            get { return SubtitleMatcherProvider.ProviderName; }
        }

        private CultureInfo _selectedLanguage;

        public CultureInfo SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { _selectedLanguage = value; }
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

        #endregion

        #region IEquatable<SubsMatchProviderViewModel> Members

       
        #endregion
    }
}
