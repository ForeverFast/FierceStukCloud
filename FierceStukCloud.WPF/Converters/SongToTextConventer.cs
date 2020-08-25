using FierceStukCloud.Core;
using System;
using System.Globalization;
using System.Windows.Data;

namespace FierceStukCloud.Wpf.Converters
{
    class SongToTextConventer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = value as Song;
             
            return temp.Author + " - " + temp.Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
