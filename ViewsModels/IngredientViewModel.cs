using NyamNyamDesktopApp.Commands;
using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class IngredientViewModel : ViewModelBase
    {
        public IngredientViewModel()
        {
            Title = "Ingredients";
        }

        private void CalculateTotalPrice()
        {
            TotalAvailableIngredientsPriceInCents = Ingredients.Sum(i => i.CountInStock * i.PricePerUnitInCents);
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

        private RelayCommand loadIngredients;

        public ICommand LoadIngredients
        {
            get
            {
                if (loadIngredients == null)
                {
                    loadIngredients = new RelayCommand(PerformLoadIngredients);
                }

                return loadIngredients;
            }
        }

        private async void PerformLoadIngredients(object commandParameter)
        {
            Ingredients = await IngredientDataStore.GetAllASync();
            CalculateTotalPrice();
        }
    }
}
