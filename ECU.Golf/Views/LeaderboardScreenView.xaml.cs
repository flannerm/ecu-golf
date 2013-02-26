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
using ECU.Golf.ViewModels;

namespace ECU.Golf.Views
{
    /// <summary>
    /// Interaction logic for LeaderboardScreenView.xaml
    /// </summary>
    public partial class LeaderboardScreenView : UserControl
    {
        public LeaderboardScreenView()
        {
            InitializeComponent();
        }

        private void lstLeaderboard_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            //Stop window from bounces when listbox upper/lower boundaries are hit
            e.Handled = true;
        }

        private void lstNotables_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            //Stop window from bounces when listbox upper/lower boundaries are hit
            e.Handled = true;
        }

        private void lstBestRounds_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            //Stop window from bounces when listbox upper/lower boundaries are hit
            e.Handled = true;
        }

        private void playerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LeaderboardScreenViewModel vm = (LeaderboardScreenViewModel)this.DataContext;

            if (vm != null)
            {
                lstLeaderboard.ScrollIntoView(vm.PlayerSearchVM);
            }
        }
    }
}
