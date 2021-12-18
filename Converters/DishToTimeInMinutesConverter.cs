using NyamNyamDesktopApp.Models.Entities;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NyamNyamDesktopApp.Converters
{
    /// <summary>
    /// Provides a way to convert a dish cooking time to minutes representation.
    /// </summary>
    public class DishToTimeInMinutesConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            Dish dish = value as Dish;
            int totalDishCookingTimeInMinutes = dish.DishStage.Sum(s => s.TimeInMinutes);
            return totalDishCookingTimeInMinutes;
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
