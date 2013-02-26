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
    /// Interaction logic for PairingsView.xaml
    /// </summary>
    public partial class PairingsScreenView : UserControl
    {
        public PairingsScreenView()
        {
            InitializeComponent();

            
        }

        private void lstPairings_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            //Stop window from bounces when listbox upper/lower boundaries are hit
            e.Handled = true;
        }

        private void playerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            PairingsScreenViewModel vm = (PairingsScreenViewModel)this.DataContext;

            if (vm != null)
            {
                lstPairings.ScrollIntoView(vm.PairingSearchVM);
            }            
        }
    }
}
