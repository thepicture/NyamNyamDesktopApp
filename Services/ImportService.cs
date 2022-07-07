using NyamNyamDesktopApp.Models.Entities;
using NyamNyamDesktopApp.Models.Factories;
using System.IO;
using System.Linq;

namespace NyamNyamDesktopApp.Services
{
    public static class ImportService
    {
        static readonly NyamNyamBaseEntities model = new NyamNyamContextFactory().Create();
     
        public static void ImportStages()
        {
            var fileData = File.ReadAllLines(@".\Stages.txt");
            int currentId = 0;
            int currentStageId = 0;
            foreach (var line in fileData)
            {
                var data = line.Split('\t');
                if (!string.IsNullOrEmpty(data[0]))
                {
                    currentId = int.Parse(data[0]);
                }
                if (!string.IsNullOrEmpty(data[1]))
                {
                    var TempStage = new DishStage
                    {
                        DishId = currentId,
                        ProcessDescription = data[1],
                        TimeInMinutes = byte.Parse(data[2].Replace(" minutes", ""))
                    };
                    model.DishStage.Add(TempStage);
                    model.SaveChanges();
                    currentStageId = model.DishStage.Where(n => n.ProcessDescription == TempStage.ProcessDescription).FirstOrDefault().StageId;
                }
                var ingredientName = data[3];
                var TempIngredientOfStage = new StageIngredient
                {
                    DishStageId = currentStageId,
                    IngredientId = model.Ingredient.Where(i => i.IngredientName == ingredientName).FirstOrDefault().IngredientId,
                    Quantity = decimal.Parse(data[4])
                };
                model.StageIngredient.Add(TempIngredientOfStage);
                model.SaveChanges();
            }
        }

        public static void ImportDishes()
        {
            var fileData = File.ReadAllLines(@".\Dishes.txt");
            foreach (var line in fileData)
            {
                var data = line.Split('\t');
                var categoryName = data[3].Trim();
                Dish dish = new Dish
                {
                    DishId = int.Parse(data[0]),
                    DishName = data[1].Trim(),
                    BaseServings = byte.Parse(data[2]),
                    CategoryId = model.DishCategory.Where(c => c.CategoryName == categoryName).FirstOrDefault().CategoryId,
                    ImagePath = data[4],
                    SourceRecipeLink = data[5],
                    Description = data[6],
                    FinalPriceInCents = int.Parse(data[7].Replace("$", "").Replace(",", ""))
                };
                model.Dish.Add(dish);
                model.SaveChanges();
            }
        }

        public static void ImportIngredients()
        {
            var fileData = File.ReadAllLines(@".\Ingredients.txt");
            foreach (var line in fileData)
            {
                var data = line.Split('\t');
                var unitData = data[2].Split(' ');

                var TempUnit = new IngredientUnit
                {
                    UnitName = unitData[0]
                };
                if (model.IngredientUnit.Where(u => u.UnitName == TempUnit.UnitName).Count() == 0)
                {
                    model.IngredientUnit.Add(TempUnit);
                    model.SaveChanges();
                }
            }
            foreach (var line in fileData)
            {
                var data = line.Split('\t');
                var unitName = data[2].Split(' ')[0];
                var tempIngredient = new Ingredient
                {
                    IngredientName = data[0].Trim(),
                    PricePerUnitInCents = data[1].Contains('$') ? int.Parse(data[1].Trim().Replace("$", "").Replace(".", "")) : int.Parse(data[1].Trim().Replace(" cents", "")),
                    CountInStock = decimal.Parse(data[3].Trim()),
                    UnitId = model.IngredientUnit.FirstOrDefault(u => u.UnitName == unitName).UnitId
                };
                model.Ingredient.Add(tempIngredient);
            }
            model.SaveChanges();
        }
    }
}