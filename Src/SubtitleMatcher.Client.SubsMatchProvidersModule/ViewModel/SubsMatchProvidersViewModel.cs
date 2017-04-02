using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SubtitlesMatcher.Common;
using SubtitleMatcher.Client.Common;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Collections.ObjectModel;

namespace SubtitleMatcher.Client.SubsMatchProvidersModule.ViewModel
{
    public class SubsMatchProvidersViewModel : ISubsMatchProvidersViewModel ,INotifyPropertyChanged
    {
        [ImportMany(typeof(ISubtitleMatcherProvider))]
        private List<ISubtitleMatcherProvider> _importedSubtitleMatcherProvider;


        private SubsMatchProviderViewModel _selectedSubtitleMatcherProvider;


        public SubsMatchProviderViewModel SelectedSubtitleMatcherProvider
        {
            get { return _selectedSubtitleMatcherProvider; }
            set
            {
                _selectedSubtitleMatcherProvider = value;
                OnPropertyChanged("SelectedSubtitleMatcherProvider");
            }
        }

        private ObservableCollection<SubsMatchProviderViewModel> _SubtitleMatcherProviders;

        
        public ObservableCollection<SubsMatchProviderViewModel> SubtitleMatcherProviders
        {
            get { return _SubtitleMatcherProviders; }
            set
            {
                _SubtitleMatcherProviders = value;
                OnPropertyChanged("SubtitleMatcherProviders");
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

        #region ISubsMatchProvidersViewModel Members

        public ISubtitleMatcherProvider TheSelectedSubtitleMatcherProvider
        {
            get { return SelectedSubtitleMatcherProvider == null ? null : SelectedSubtitleMatcherProvider.SubtitleMatcherProvider; }
        }

        public CultureInfo SelectedLanguage
        {
            get { return SelectedSubtitleMatcherProvider == null ? null : SelectedSubtitleMatcherProvider.SelectedLanguage; }
        }

        #endregion

        public void InitSubsMatchProviders()
        {
            if (_importedSubtitleMatcherProvider != null && _importedSubtitleMatcherProvider.Count > 0)
            {
                SubtitleMatcherProviders = new ObservableCollection<SubsMatchProviderViewModel>();
                _importedSubtitleMatcherProvider.ForEach(pr => 
                    {
                        SubtitleMatcherProviders.Add(new SubsMatchProviderViewModel() { SubtitleMatcherProvider = pr });
                    });


                SelectedSubtitleMatcherProvider = SubtitleMatcherProviders[0];
            }
        }
    }
}
