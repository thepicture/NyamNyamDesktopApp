using System;
using System.Globalization;
using System.Windows.Data;

namespace NyamNyamDesktopApp.Converters
{
    class CentsToDollarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value * 1.0 / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
