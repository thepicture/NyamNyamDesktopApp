using NyamNyamDesktopApp.Commands;
using NyamNyamDesktopApp.Models;
using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class RecipeViewModel : ViewModelBase
    {
        private Dish _currentDish;
        private IList<ExtendedIngredient> _ingredients;
        private IEnumerable<string> _cookingProcess;
        private double _totalCostInCents;

        public RecipeViewModel(Dish dish)
        {
            Title = "Recipes";
            CurrentDish = dish;
            Ingredients = DishIngredientsChecker.GetIngredientsWithAvailabilityOfDish(dish)
                                                .ToList();
            int stageId = 1;
            CookingProcess = dish.DishStage.Select(d => stageId++ + ". " + d.ProcessDescription);
            TotalCostInCents = dish.FinalPriceInCents;
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
        public IEnumerable<string> CookingProcess
        {
            get => _cookingProcess;
            set => SetProperty(ref _cookingProcess, value);
        }

        private RelayCommand _lessServingsCommand;

        public ICommand LessServingsCommand
        {
            get
            {
                if (_lessServingsCommand == null)
                {
                    _lessServingsCommand = new RelayCommand(LessServings, obj => ServingsCount > 1);
                }

                return _lessServingsCommand;
            }
        }

        private void LessServings(object commandParameter)
        {
            ServingsCount--;
            TotalCostInCents = CurrentDish.FinalPriceInCents * ServingsCount;
        }

        private int _servingsCount = 1;

        public int ServingsCount
        {
            get => _servingsCount;
            set => SetProperty(ref _servingsCount, value);
        }

        private RelayCommand _moreServingsCommand;

        public ICommand MoreServingsCommand
        {
            get
            {
                if (_moreServingsCommand == null)
                {
                    _moreServingsCommand = new RelayCommand(MoreServings);
                }

                return _moreServingsCommand;
            }
        }

        public double TotalCostInCents
        {
            get => _totalCostInCents;
            set => SetProperty(ref _totalCostInCents, value);
        }

        private void MoreServings(object commandParameter)
        {
            ServingsCount++;
            TotalCostInCents = CurrentDish.FinalPriceInCents * ServingsCount;
        }
    }
}