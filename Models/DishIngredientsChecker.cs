using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NyamNyamDesktopApp.Models
{
    public static class DishIngredientsChecker
    {
        public static bool IsPossibleToCook(Dish dish)
        {
            if (dish == null)
            {
                return false;
            }
            bool isPossibleToCook = GetIngredientsWithAvailabilityOfDish(dish, 1).Count(i => !i.IsAvailable) == 0;
            return isPossibleToCook;
        }

        public static IEnumerable<ExtendedIngredient> GetIngredientsWithAvailabilityOfDish(Dish dish,
                                                                                           int servingsCount)
        {
            var dishStages = new List<DishStage>();
            var stageIngredients = new List<StageIngredient>();
            var ingredients = new List<Ingredient>();

            foreach (var dishStage in dish.DishStage)
            {
                dishStages.Add(dishStage);
                foreach (var stageIngredient in dishStage.StageIngredient)
                {
                    stageIngredients.Add(stageIngredient);
                    if (!ingredients.Contains(stageIngredient.Ingredient))
                    {
                        ingredients.Add(stageIngredient.Ingredient);
                    }
                }
            }

            return (from ds in dish.DishStage
                    join si in stageIngredients on ds.StageId equals si.DishStageId
                    join i in ingredients on si.IngredientId equals i.IngredientId
                    group si by (
                        si.Ingredient,
                        si.Quantity,
                        i.CountInStock
                    ) into iq
                    select new ExtendedIngredient
                    (
                        iq.Sum(stageIngredient => stageIngredient.Quantity * servingsCount) <= iq.Key.CountInStock,
                        iq.Key.Ingredient.IngredientName,
                        iq.Key.CountInStock - iq.Sum(stageIngredient => stageIngredient.Quantity * servingsCount),
                        iq.Key.Ingredient.IngredientUnit.UnitName,
                        iq.Key.Ingredient.PricePerUnitInCents * servingsCount
                    )).ToList();
        }
    }
}
