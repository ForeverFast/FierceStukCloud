using System;
using System.Globalization;
using System.Windows.Data;

namespace FierceStukCloud.Wpf.Converters
{
    public class WidthWindowToWidthGridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((double)value >= System.Convert.ToDouble(parameter))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
