using NyamNyamDesktopApp.Models.Entities;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NyamNyamDesktopApp.Converters
{
    public class DishToTimeInMinutesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Dish dish = value as Dish;
            return dish.DishStage.Sum(s => s.TimeInMinutes);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
