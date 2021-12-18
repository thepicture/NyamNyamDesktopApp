using NyamNyamDesktopApp.Commands;
using NyamNyamDesktopApp.Models;
using NyamNyamDesktopApp.Models.Entities;
using NyamNyamDesktopApp.Models.Factories;
using NyamNyamDesktopApp.Services;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class DishViewModel : ViewModelBase
    {
        private IEnumerable<Dish> _dishes;
        private IEnumerable<DishCategory> _categories;
        private DishCategory _currentCategory;
        private string _nameOrDescriptionSearchText = string.Empty;
        private double _minPriceInDollars = 1;
        private double _maxPriceInDollars = 100;
        private readonly IDbContextFactory<NyamNyamBaseEntities> _databaseFactory;
        public DishViewModel()
        {
            Title = "Dishes";
            _databaseFactory = DependencyService
                               .Get<IDbContextFactory<NyamNyamBaseEntities>>();
        }

        private async void GetDishCategories()
        {
            using (NyamNyamBaseEntities context = _databaseFactory.Create())
            {
                IEnumerable<DishCategory> extendedCategories =
                    await context.DishCategory
                                 .ToListAsync();
                extendedCategories = extendedCategories.Prepend(new DishCategory
                {
                    CategoryName = "All categories"
                });
                Categories = extendedCategories;
                CurrentCategory = extendedCategories.First();
            }
        }

        public IEnumerable<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }

        public IEnumerable<DishCategory> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }
        public DishCategory CurrentCategory
        {
            get => _currentCategory;
            set
            {
                _ = SetProperty(ref _currentCategory, value);
                FilterDishes();
            }
        }

        public string NameOrDescriptionSearchText
        {
            get => _nameOrDescriptionSearchText;
            set
            {
                _ = SetProperty(ref _nameOrDescriptionSearchText, value);
                FilterDishes();
            }
        }

        private bool _areOnlyAvailableIngredientsDishes = false;

        public bool AreOnlyAvailableIngredientsDishes
        {
            get => _areOnlyAvailableIngredientsDishes;
            set
            {
                _ = SetProperty(ref _areOnlyAvailableIngredientsDishes, value);
                FilterDishes();
            }
        }
        public double MinPriceInDollars
        {
            get => _minPriceInDollars;
            set
            {
                _ = SetProperty(ref _minPriceInDollars, value);
                FilterDishes();
            }
        }
        public double MaxPriceInDollars
        {
            get => _maxPriceInDollars;
            set
            {
                SetProperty(ref _maxPriceInDollars, value);
                FilterDishes();
            }
        }

        private async void FilterDishes()
        {
            NyamNyamBaseEntities context = _databaseFactory.Create();
            IEnumerable<Dish> dishesFromDatabase = await context.Dish.ToListAsync();

            if (CurrentCategory.CategoryName != "All categories")
            {
                dishesFromDatabase = dishesFromDatabase.Where(d =>
                {
                    return d.DishCategory.CategoryId == CurrentCategory.CategoryId;
                });
            }
            if (!string.IsNullOrWhiteSpace(NameOrDescriptionSearchText))
            {
                dishesFromDatabase = dishesFromDatabase.Where(d =>
                {
                    return d.DishName.ToLower()
                                     .Contains(NameOrDescriptionSearchText)
                                     || d.Description
                                         .ToLower()
                                         .Contains(NameOrDescriptionSearchText);
                });
            }
            if (AreOnlyAvailableIngredientsDishes)
            {
                dishesFromDatabase = dishesFromDatabase.Where(d =>
                {
                    return DishCookPosibilityChecker.IsPossibleToCook(d);
                });
            }
            dishesFromDatabase = dishesFromDatabase.Where(d =>
            {
                return d.FinalPriceInCents >= MinPriceInDollars * 100
                       && d.FinalPriceInCents <= MaxPriceInDollars * 100;
            });

            Dishes = dishesFromDatabase;
        }

        private RelayCommand navigateToDishRecipeCommand;

        public ICommand NavigateToDishRecipeCommand
        {
            get
            {
                if (navigateToDishRecipeCommand == null)
                {
                    navigateToDishRecipeCommand =
                        new RelayCommand(NavigateToDishRecipe);
                }

                return navigateToDishRecipeCommand;
            }
        }

        /// <summary>
        /// Performs navigation to dish recipe view model.
        /// </summary>
        /// <param name="commandParameter">The command parameter.</param>
        private void NavigateToDishRecipe(object commandParameter)
        {
            Dish dish = commandParameter as Dish;
            DependencyService.Get<INavigationService>()
                             .NavigateWithParameter<RecipeViewModel>(dish);
        }

        private RelayCommand loadDishesCommand;

        public ICommand LoadDishesCommand
        {
            get
            {
                if (loadDishesCommand == null)
                {
                    loadDishesCommand = new RelayCommand(LoadDishesAndCategories);
                }

                return loadDishesCommand;
            }
        }

        /// <summary>
        /// Performs filling <see cref="Dishes"/> 
        /// with the <see cref="NyamNyamBaseEntities"/>
        /// <see cref="DbSet"/>.
        /// </summary>
        /// <param name="commandParameter">The command parameter.</param>
        private async void LoadDishesAndCategories(object commandParameter)
        {
            NyamNyamBaseEntities context = _databaseFactory.Create();
            Dishes = await context.Dish.ToListAsync();
            GetDishCategories();
        }
    }
}
