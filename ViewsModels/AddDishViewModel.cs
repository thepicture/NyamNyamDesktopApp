using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class AddDishViewModel : ViewModelBase
    {
        private Dish _currentDish;
        private IEnumerable<DishCategory> _dishCategories;
        public AddDishViewModel()
        {
            Title = "Add a new dish";
            CurrentDish = new Dish();
        }

        public AddDishViewModel(Dish dish) : this()
        {
            CurrentDish = dish;
        }

        public Dish CurrentDish
        {
            get => _currentDish;
            set => SetProperty(ref _currentDish, value);
        }

        private DishCategory _currentDishCategory;

        public DishCategory CurrentDishCategory
        {
            get => _currentDishCategory;
            set => SetProperty(ref _currentDishCategory, value);
        }
        public IEnumerable<DishCategory> DishCategories
        {
            get => _dishCategories;
            set => SetProperty(ref _dishCategories, value);
        }

        private int _totalPriceInCents;

        public int TotalPriceInCents
        {
            get => _totalPriceInCents;
            set => SetProperty(ref _totalPriceInCents, value);
        }
    }
}
