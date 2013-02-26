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
using Microsoft.Maps.MapControl.WPF.Design;
using Microsoft.Maps.MapControl.WPF;
using System.Configuration;

namespace ECU.Golf.Views
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();

            string[] coords = AppData.Tournament.Event.Courses[0].CourseGpsLocation.Split(',');
            double latitude = double.Parse(coords[0]);
            double longitude = double.Parse(coords[1]);

            myMap.Center = new Location(latitude, longitude);
            myMap.Heading = AppData.Tournament.Event.Courses[0].CourseGpsHeading;            
        }

        private void lstLeaderboard_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            //Stop window from bounces when listbox upper/lower boundaries are hit
            e.Handled = true;
        }


        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            myMap.ZoomLevel = myMap.ZoomLevel + 1;
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            myMap.ZoomLevel = myMap.ZoomLevel - 1;
        }

    }
}
