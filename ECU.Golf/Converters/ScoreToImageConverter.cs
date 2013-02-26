using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ECU.Golf.Converters
{
    public class ScoreToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //BitmapImage bitmap = null;
            ImageSource imageSource = null;
            
            try
            {
                if (value == null)
                {
                    imageSource = new BitmapImage();
                }
                else
                {
                    switch (value.ToString())
                    {
                        case "-4":
                            imageSource = new BitmapImage(new Uri("/ECU.Golf;component/Images/eagle.png", UriKind.RelativeOrAbsolute));
                            break;
                        case "-3":
                            imageSource = new BitmapImage(new Uri("/ECU.Golf;component/Images/eagle.png", UriKind.RelativeOrAbsolute));
                            break;
                        case "-2":
                            imageSource = new BitmapImage(new Uri("/ECU.Golf;component/Images/eagle.png", UriKind.RelativeOrAbsolute));
                            break;
                        case "-1":
                            imageSource = new BitmapImage(new Uri("/ECU.Golf;component/Images/birdie.png", UriKind.RelativeOrAbsolute));
                            break;
                        case "0":
                            imageSource = new BitmapImage();
                            break;
                        case "1":
                            imageSource = new BitmapImage(new Uri("/ECU.Golf;component/Images/bogey.png", UriKind.RelativeOrAbsolute));
                            break;
                        default:
                            imageSource = new BitmapImage(new Uri("/ECU.Golf;component/Images/other.png", UriKind.RelativeOrAbsolute));
                            break;                        
                    }                
                }               
            }
            catch (Exception ex)
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
