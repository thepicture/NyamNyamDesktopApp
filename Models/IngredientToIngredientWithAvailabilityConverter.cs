using NyamNyamDesktopApp.Models;
using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NyamNyamDesktopApp.Models
{
    /// <summary>
    /// Static class for converting dish ingredients to an 
    /// ingredient with availability representation.
    /// </summary>
    public static class IngredientToIngredientWithAvailabilityConverter
    {
        /// <summary>
        /// Converts dish ingredients to an ingredient with availability 
        /// representation with respect to servings count.
        /// </summary>
        /// <param name="dish">The dish whose ingredients will be converted.</param>
        /// <param name="servingsCount">The servings count of the given dish.</param>
        /// <returns></returns>
        public static IEnumerable<ExtendedIngredient>
           ConvertDishIngredientsToIngredientsWithAvailability(Dish dish,
                                                               int servingsCount)
        {
            List<DishStage> dishStages = new List<DishStage>();
            List<StageIngredient> stageIngredients = new List<StageIngredient>();
            List<Ingredient> ingredients = new List<Ingredient>();

            foreach (DishStage dishStage in dish.DishStage)
            {
                dishStages.Add(dishStage);
                foreach (StageIngredient stageIngredient in dishStage.StageIngredient)
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
                        iq.Sum(stageIngredient => stageIngredient.Quantity
                        * servingsCount) <= iq.Key.CountInStock,
                        iq.Key.Ingredient.IngredientName,
                        iq.Key.CountInStock - iq.Sum(stageIngredient =>
                        {
                            return stageIngredient.Quantity
                                                    * servingsCount;
                        }),
                        iq.Key.Ingredient.IngredientUnit.UnitName,
                        iq.Key.Ingredient.PricePerUnitInCents * servingsCount
                    )).ToList();
        }
    }
}
