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
    /// Interaction logic for PairingView.xaml
    /// </summary>
    public partial class PairingView : UserControl
    {
        public PairingView()
        {
            InitializeComponent();
        }

        //private void ItemsControl_MouseDown(object sender, MouseButtonEventArgs e)
        //{
            //ItemsControl ic = (ItemsControl)e.Source;

            //PlayerTrackingViewModel playerVM = (PlayerTrackingViewModel)content.Item

            //PairingViewModel pairingVM = (PairingViewModel)this.DataContext;

            //trackingHoleVM.SelectedPlayer = playerVM;
        //}
    }
}
