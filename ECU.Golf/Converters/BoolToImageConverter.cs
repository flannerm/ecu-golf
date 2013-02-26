using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace ECU.Golf.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource imageSource = null;

            try
            {
                if (value == null)
                {
                    imageSource = new BitmapImage();
                }
                else
                {
                    switch (value.ToString().ToUpper())
                    {
                        case "TRUE":
                            imageSource = new BitmapImage(new Uri("/ECU.Golf;component/Images/made.png", UriKind.RelativeOrAbsolute));
                            break;
                        case "FALSE":
                            imageSource = new BitmapImage(new Uri("/ECU.Golf;component/Images/miss.png", UriKind.RelativeOrAbsolute));
                            break;
                        default:
                            imageSource = new BitmapImage();
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }

            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
