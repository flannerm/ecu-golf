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
    /// Interaction logic for TwitterScreenView.xaml
    /// </summary>
    public partial class TwitterScreenView : UserControl
    {
        public TwitterScreenView()
        {
            InitializeComponent();
        }

        private void lstTweets_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            //Stop window from bounces when listbox upper/lower boundaries are hit
            e.Handled = true;
        }
    }
}
