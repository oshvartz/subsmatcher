using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SubtitleMatcher.Client.Common;

namespace SubtitleMatcher.Client.SubsMatchProvidersModule.Views
{
    /// <summary>
    /// Interaction logic for SubsMatchProvidersView.xaml
    /// </summary>
    public partial class SubsMatchProvidersView : UserControl
    {
        public SubsMatchProvidersView(ISubsMatchProvidersViewModel subsMatchProvidersViewModel)
        {
            InitializeComponent();
            DataContext = subsMatchProvidersViewModel;
            
        }
    }
}
