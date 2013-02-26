using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ECU.Golf.Converters
{
    public class BoolToColorTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush selectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")); 
            Brush unselectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));

            bool b = (bool)value;

            selectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
           
            return b ? selectedBrush : unselectedBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
