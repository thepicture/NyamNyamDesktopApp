using NyamNyamDesktopApp.Models;
using NyamNyamDesktopApp.Models.Entities;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NyamNyamDesktopApp.Converters
{
    public class DishColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Dish dish = value as Dish;
            bool isEnoughIngredients = DishIngredientsExistChecker.Check(dish);
            return isEnoughIngredients ? PixelFormats.Pbgra32 : PixelFormats.Gray8;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
