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

namespace ECU.Golf.Views
{
    /// <summary>
    /// Interaction logic for HistoryScreenView.xaml
    /// </summary>
    public partial class HistoryScreenView : UserControl
    {
        public HistoryScreenView()
        {
            InitializeComponent();
        }

        private void lstHistory_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            //Stop window from bounces when listbox upper/lower boundaries are hit
            e.Handled = true;
        }
    }
}
