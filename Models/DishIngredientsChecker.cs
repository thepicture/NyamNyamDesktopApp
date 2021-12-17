using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NyamNyamDesktopApp.Models
{
    public static class DishIngredientsChecker
    {
        public static bool IsPossibleToCook(Dish dish)
        {
            NyamNyamBaseEntities db = new NyamNyamBaseEntities();
            if (dish == null)
            {
                return false;
            }
            bool isPossibleToCook = GetIngredientsWithAvailabilityOfDish(dish).Count(i => !i.IsAvailable) == 0;
            return isPossibleToCook;
        }

        public static IEnumerable<ExtendedIngredient> GetIngredientsWithAvailabilityOfDish(Dish dish)
        {
            NyamNyamBaseEntities db = new NyamNyamBaseEntities();
            return (from ds in dish.DishStage
                    join si in db.StageIngredient on ds.StageId equals si.DishStageId
                    join i in db.Ingredient on si.IngredientId equals i.IngredientId
                    group si by new
                    {
                        si.Ingredient,
                        si.Quantity,
                        i.CountInStock,
                    } into iq
                    select new ExtendedIngredient
                    (
                        iq.Key.Ingredient,
                        iq.Sum(stageIngredient => stageIngredient.Quantity) <= iq.Key.CountInStock
                    )).ToList();
        }
    }
}
