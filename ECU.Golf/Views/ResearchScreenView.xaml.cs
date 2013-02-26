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
    /// Interaction logic for ResearchScreenView.xaml
    /// </summary>
    public partial class ResearchScreenView : UserControl
    {
        public ResearchScreenView()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ResearchScreenViewModel vm = (ResearchScreenViewModel)this.DataContext;

            vm.ResearchNoteHasFocus = true;
        }

       
    }
}
