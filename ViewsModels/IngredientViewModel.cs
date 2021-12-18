using NyamNyamDesktopApp.Commands;
using NyamNyamDesktopApp.Models.Entities;
using NyamNyamDesktopApp.Models.Factories;
using NyamNyamDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class IngredientViewModel : ViewModelBase
    {
        private readonly IDbContextFactory<NyamNyamBaseEntities> _dbFactory;
        private ObservableCollection<IngredientTemplateViewModel> _ingredientViewModels;
        public IngredientViewModel()
        {
            Title = "Ingredients";
            _dbFactory = DependencyService.Get<NyamNyamContextFactory>();
        }

        private void CalculateTotalPriceOfAllIngredients()
        {
            TotalAvailableIngredientsPriceInCents = IngredientViewModels.Sum(i =>
            {
                return Convert.ToInt32(
                    Math.Ceiling(i.CurrentIngredient.CountInStock
                                 * i.CurrentIngredient.PricePerUnitInCents)
                    );
            });
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
            List<Ingredient> ingredients = await _dbFactory.Create().Ingredient
                .ToListAsync();
            IngredientViewModels =
                new ObservableCollection<IngredientTemplateViewModel>(
                    ingredients
                    .Select(i => new IngredientTemplateViewModel(i)).ToList()
                );
            foreach (IngredientTemplateViewModel ingredientViewModel
                                                 in IngredientViewModels)
            {
                ingredientViewModel.PropertyChanged +=
                    OnIngredientTemplateViewModelChanged;
            }

            CalculateTotalPriceOfAllIngredients();
        }

        private void OnIngredientTemplateViewModelChanged(object sender,
                                                          PropertyChangedEventArgs e)
        {
            CalculateTotalPriceOfAllIngredients();
        }

        public ObservableCollection<IngredientTemplateViewModel> IngredientViewModels
        {
            get => _ingredientViewModels;
            set
            {
                SetProperty(ref _ingredientViewModels, value);
                CalculateTotalPriceOfAllIngredients();
            }
        }

        private RelayCommand deleteIngredientCommand;

        public ICommand DeleteIngredientCommand
        {
            get
            {
                if (deleteIngredientCommand == null)
                {
                    deleteIngredientCommand = new RelayCommand(DeleteIngredient);
                }

                return deleteIngredientCommand;
            }
        }

        private void DeleteIngredient(object commandParameter)
        {
            IngredientTemplateViewModel templateViewModel =
                commandParameter as IngredientTemplateViewModel;
            Ingredient ingredientToDelete = templateViewModel.CurrentIngredient;
            if (ingredientToDelete.StageIngredient.Count > 0)
            {
                IEnumerable<Dish> dishesWhereIngredientExists =
                    ingredientToDelete
                    .StageIngredient
                    .Select(s => s.DishStage.Dish)
                    .Distinct();
                DependencyService.Get<IFeedbackService>().ShowWarning("Impossible " +
                    "to delete, " +
                    "the number of dishes in which the ingredient is used: "
                    + dishesWhereIngredientExists.Count());
                return;
            }
            NyamNyamBaseEntities context = _dbFactory.Create();
            Ingredient ingredientFromDatabaseToDelete = context
                .Ingredient.Find(ingredientToDelete.IngredientId);
            context.Ingredient.Remove(ingredientFromDatabaseToDelete);
            try
            {
                context.SaveChanges();
                IngredientViewModels.Remove(templateViewModel);
                DependencyService.Get<IFeedbackService>().ShowInformation("The ingredient "
                    + ingredientToDelete.IngredientName
                    + " was successfully deleted");
            }
            catch (DataException)
            {
                DependencyService.Get<IFeedbackService>().ShowError("Can't " +
                    "delete ingredient. Try again. If this doesn't help, " +
                    "restart the app or try to change pages");
            }
        }
    }
}
