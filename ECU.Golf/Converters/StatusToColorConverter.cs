using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ECU.Golf.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f9d05e"));

            switch (value.ToString().ToUpper())
            {
                case "CUT":
                case "WD":
                case "DQ":
                    color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff0000"));
                    break;
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
