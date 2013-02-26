using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ECU.Golf.Converters
{
    public class ZeroScoreConverter : IValueConverter
    {        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string val = "";

            try
            {
                
                if (value == null || value.ToString() == "0")
                {
                    val = "";
                }
                else
                {
                    val = value.ToString();
                }
            }
            catch (Exception)
            {
            }
            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
    }
}
