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
    /// Interaction logic for TrackingHoleView.xaml
    /// </summary>
    public partial class TrackingHoleView : UserControl
    {
        public TrackingHoleView()
        {
            InitializeComponent();
        }

        private void ContentControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentControl content = (ContentControl)sender;

            PlayerTrackingViewModel playerVM = (PlayerTrackingViewModel)content.Content;

            TrackingHoleViewModel trackingHoleVM = (TrackingHoleViewModel)this.DataContext;

            trackingHoleVM.SelectedPlayer = playerVM;

        }

    }
}
