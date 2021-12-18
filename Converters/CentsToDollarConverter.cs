using System;
using System.Globalization;
using System.Windows.Data;

namespace NyamNyamDesktopApp.Converters
{
    /// <summary>
    /// Provides a way to convert a cents value to a dollars exchange rate.
    /// </summary>
    public class CentsToDollarConverter : IValueConverter
    {
        private const int centsInOneDollar = 100;

        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            double cents = System.Convert.ToDouble(value);
            return cents / centsInOneDollar;
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
