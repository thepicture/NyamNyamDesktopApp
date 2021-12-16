using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class RecipeViewModel : ViewModelBase
    {
        private Dish _currentDish;
        private IList<Ingredient> _ingredients;

        public RecipeViewModel(Dish dish)
        {
            Title = "Recipes";
            CurrentDish = dish;
            Ingredients = new List<Ingredient>();
            var dishStages = CurrentDish.DishStage;
            foreach (DishStage stage in dishStages)
            {
                foreach (var stageIngredient in stage.StageIngredient)
                {
                    if (!Ingredients.Contains(stageIngredient.Ingredient))
                    {
                        Ingredients.Add(stageIngredient.Ingredient);
                    }
                }
            }
        }

        public Dish CurrentDish
        {
            get => _currentDish;
            set => SetProperty(ref _currentDish, value);
        }
        public IList<Ingredient> Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }
    }
}