using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NyamNyamDesktopApp.Models
{
    /// <summary>
    /// Implements the method for checking a dish cooking posibility.
    /// </summary>
    public static class DishCookPosibilityChecker
    {
        /// <summary>
        /// Checks if there are enough ingredients 
        /// for cooking the given dish.
        /// </summary>
        /// <param name="dish">The given dish.</param>
        /// <returns>True if possible to cook, otherwise false.</returns>
        public static bool IsPossibleToCook(Dish dish)
        {
            if (dish == null)
            {
                return false;
            }

            IEnumerable<ExtendedIngredient> ingredients =
                IngredientToIngredientWithAvailabilityConverter
                .ConvertDishIngredientsToIngredientsWithAvailability(dish, 1);

            bool isPossibleToCook = ingredients.All(i => i.IsAvailable);
            return isPossibleToCook;
        }
    }
}
