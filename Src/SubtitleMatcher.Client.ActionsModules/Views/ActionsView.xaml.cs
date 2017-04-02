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
using SubtitleMatcher.Client.ActionsModules.ViewModels;

namespace SubtitleMatcher.Client.ActionsModules.Views
{
    /// <summary>
    /// Interaction logic for ActionsView.xaml
    /// </summary>
    public partial class ActionsView : UserControl
    {
        public ActionsView(ActionsViewModel actionsViewModel)
        {
            InitializeComponent();
            DataContext = actionsViewModel;
        }
    }
}
