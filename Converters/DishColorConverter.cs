using NyamNyamDesktopApp.Models;
using NyamNyamDesktopApp.Models.Entities;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NyamNyamDesktopApp.Converters
{
    /// <summary>
    /// Provdes a way to decorate a view into gray if there are not enough ingredients 
    /// and to return a default view otherwise.
    /// </summary>
    public class DishColorConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            Dish dish = value as Dish;
            bool isEnoughIngredients = DishCookPosibilityChecker.IsPossibleToCook(dish);
            return isEnoughIngredients ? PixelFormats.Pbgra32 : PixelFormats.Gray8;

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
