using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ECU.Golf.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            SolidColorBrush newBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));

            if (value != null)
            {
                newBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value.ToString()));
            }

            return newBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
