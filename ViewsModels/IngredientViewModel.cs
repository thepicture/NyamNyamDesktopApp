using NyamNyamDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class IngredientViewModel : ViewModelBase
    {
        public IngredientViewModel()
        {
            Title = "Ingredients";
            _ = Task.Run(LoadIngredients);
        }

        private void CalculateTotalPrice()
        {
            TotalAvailableIngredientsPriceInCents = Ingredients.Sum(i => i.CountInStock * i.PricePerUnitInCents);
        }

        private async void LoadIngredients()
        {
            Ingredients = await IngredientDataStore.GetAllASync();
            CalculateTotalPrice();
        }

        private IEnumerable<Ingredient> _ingredients;

        public IEnumerable<Ingredient> Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }

        private int _totalAvailableIngredientsPriceInCents;

        public int TotalAvailableIngredientsPriceInCents
        {
            get => _totalAvailableIngredientsPriceInCents;
            set => SetProperty(ref _totalAvailableIngredientsPriceInCents, value);
        }
    }
}
