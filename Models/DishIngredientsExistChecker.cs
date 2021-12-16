using NyamNyamDesktopApp.Models.Entities;
using System.Linq;

namespace NyamNyamDesktopApp.Models
{
    public static class DishIngredientsExistChecker
    {
        public static bool Check(Dish dish)
        {
            if (dish == null)
            {
                return false;
            }
            return dish.DishStage.All(stage => stage.StageIngredient.All(ingredient => ingredient.Quantity <= ingredient.Ingredient.CountInStock));
        }
    }
}
