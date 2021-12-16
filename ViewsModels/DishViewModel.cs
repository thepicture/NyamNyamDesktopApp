using NyamNyamDesktopApp.Models;
using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public DishViewModel()
        {
            Title = "Dishes";
            _ = Task.Run(() => GetDishes()).ContinueWith(t => GetDishCategories());
        }

        private async void GetDishCategories()
        {
            IEnumerable<DishCategory> extendedCategories = await DishCategoryDataStore.GetAllASync();
            extendedCategories = extendedCategories.ToList().Prepend(new DishCategory { CategoryName = "All categories" });
            Categories = extendedCategories;
            CurrentCategory = extendedCategories.First();
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
                SetProperty(ref _currentCategory, value);
                FilterDishes();
            }
        }

        public string NameOrDescriptionSearchText
        {
            get => _nameOrDescriptionSearchText;
            set
            {
                SetProperty(ref _nameOrDescriptionSearchText, value);
                FilterDishes();
            }
        }

        private async void GetDishes()
        {
            Dishes = await DishDataStore.GetAllASync();
        }

        private bool _areOnlyAvailableIngredientsDishes = false;

        public bool AreOnlyAvailableIngredientsDishes
        {
            get => _areOnlyAvailableIngredientsDishes;
            set
            {
                SetProperty(ref _areOnlyAvailableIngredientsDishes, value);
                FilterDishes();
            }
        }
        public double MinPriceInDollars
        {
            get => _minPriceInDollars;
            set
            {
                SetProperty(ref _minPriceInDollars, value);
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
            IEnumerable<Dish> dishesFromDatabase = await DishDataStore.GetAllASync();

            if (CurrentCategory.CategoryName != "All categories")
            {
                dishesFromDatabase = dishesFromDatabase.Where(d => d.DishCategory.CategoryId == CurrentCategory.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(NameOrDescriptionSearchText))
            {
                dishesFromDatabase = dishesFromDatabase.Where(d => d.DishName.ToLower().Contains(NameOrDescriptionSearchText) || d.Description.ToLower().Contains(NameOrDescriptionSearchText));
            }
            if (AreOnlyAvailableIngredientsDishes)
            {
                dishesFromDatabase = dishesFromDatabase.Where(d => DishIngredientsExistChecker.Check(d));
            }
            dishesFromDatabase = dishesFromDatabase.Where(d => d.FinalPriceInCents >= MinPriceInDollars * 100 && d.FinalPriceInCents <= MaxPriceInDollars * 100);

            Dishes = dishesFromDatabase;
        }
    }
}
