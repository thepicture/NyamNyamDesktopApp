using NyamNyamDesktopApp.Commands;
using NyamNyamDesktopApp.Models.Entities;
using NyamNyamDesktopApp.Models.Factories;
using NyamNyamDesktopApp.Services;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class IngredientViewModel : ViewModelBase
    {
        private readonly IDbContextFactory<NyamNyamBaseEntities> _dbFactory;
        public IngredientViewModel()
        {
            Title = "Ingredients";
            _dbFactory = DependencyService.Get<NyamNyamContextFactory>();
        }

        private void CalculateTotalPriceOfAllIngredients()
        {
            TotalAvailableIngredientsPriceInCents = Ingredients.Sum(i =>
            {
                return i.CountInStock * i.PricePerUnitInCents;
            });
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

        /// <summary>
        /// Performs filling <see cref="Ingredients"/> 
        /// with the <see cref="NyamNyamBaseEntities"/>
        /// <see cref="DbSet"/>.
        /// </summary>
        /// <param name="commandParameter">The command parameter.</param>
        private async void PerformLoadIngredients(object commandParameter)
        {
            NyamNyamBaseEntities context = _dbFactory.Create();
            Ingredients = await context.Ingredient.ToListAsync();
            CalculateTotalPriceOfAllIngredients();
        }
    }
}
