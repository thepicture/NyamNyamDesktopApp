using NyamNyamDesktopApp.Models;
using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class RecipeViewModel : ViewModelBase
    {
        private Dish _currentDish;
        private IList<ExtendedIngredient> _ingredients;

        public RecipeViewModel(Dish dish)
        {
            Title = "Recipes";
            CurrentDish = dish;
            Ingredients = DishIngredientsChecker.GetIngredientsWithAvailabilityOfDish(dish).ToList();
        }

        public Dish CurrentDish
        {
            get => _currentDish;
            set => SetProperty(ref _currentDish, value);
        }
        public IList<ExtendedIngredient> Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }
    }
}