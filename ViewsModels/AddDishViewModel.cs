using NyamNyamDesktopApp.Commands;
using NyamNyamDesktopApp.Models;
using NyamNyamDesktopApp.Models.Entities;
using NyamNyamDesktopApp.Models.Factories;
using NyamNyamDesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class AddDishViewModel : ViewModelBase
    {
        private Dish _currentDish;
        private IEnumerable<DishCategory> _dishCategories;
        private IEnumerable<Ingredient> _ingredients;
        private ObservableCollection<EnumeratedDishStage> _enumeratedDishStages;
        private readonly IDbContextFactory<NyamNyamBaseEntities> _databaseFactory;
        public AddDishViewModel()
        {
            Title = "Add a new dish";
            _databaseFactory = DependencyService
                               .Get<IDbContextFactory<NyamNyamBaseEntities>>();
            _context = _databaseFactory.Create();
        }

        private void UpdateTotalPrice(object state)
        {
            TotalPriceInCents = (int)Math.Ceiling(EnumeratedDishStages.Sum(ds =>
            {
                return ds.DishStage.StageIngredient
                .Sum(si => si.Ingredient.PricePerUnitInCents * si.Quantity) * CurrentDish.BaseServings;
            }));
        }

        public Dish CurrentDish
        {
            get => _currentDish;
            set => SetProperty(ref _currentDish, value);
        }

        private readonly NyamNyamBaseEntities _context;
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

        private RelayCommand _loadCategories;

        public ICommand LoadCategories
        {
            get
            {
                if (_loadCategories == null)
                {
                    _loadCategories =
                        new RelayCommand(PerformLoadCategoriesAndIngredients);
                }

                return _loadCategories;
            }
        }

        public IEnumerable<Ingredient> Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }
        public ObservableCollection<EnumeratedDishStage> EnumeratedDishStages
        {
            get => _enumeratedDishStages;
            set => SetProperty(ref _enumeratedDishStages, value);
        }

        private Timer _timer;

        private async void PerformLoadCategoriesAndIngredients(object commandParameter)
        {
            CurrentDish = new Dish();

            DishCategories = await _context.DishCategory.ToListAsync();
            CurrentDishCategory = DishCategories.First();

            Ingredients = await _context.Ingredient.ToListAsync();
            foreach (DishStage stage in CurrentDish.DishStage)
            {
                foreach (StageIngredient stageIngredient in stage.StageIngredient)
                {
                    foreach (Ingredient ingredient in Ingredients)
                    {
                        if (ingredient.IngredientId == stageIngredient.IngredientId)
                        {
                            stageIngredient.Ingredient = ingredient;
                        }
                    }
                }
            }
            int stageNumber = 1;
            EnumeratedDishStages =
                new ObservableCollection<EnumeratedDishStage>
                (CurrentDish.DishStage.Select(s =>
                {
                    return new EnumeratedDishStage(s, "Stage "
                                                      + stageNumber++.ToString());
                }));

            _timer =
                new Timer(UpdateTotalPrice);
            if (!_timer.Change(0, (int)TimeSpan.FromSeconds(1).TotalMilliseconds))
            {
                DependencyService.Get<IFeedbackService>().ShowError("Can't prepare " +
                    "the page for adding a new dish. You will be navigated " +
                    "to previous page. Try to open this page again soon.");
                DependencyService.Get<INavigationService>().GoBack();
            }
        }

        private RelayCommand _deleteStageCommand;

        public ICommand DeleteStageCommand
        {
            get
            {
                if (_deleteStageCommand == null)
                {
                    _deleteStageCommand = new RelayCommand(DeleteStage);
                }

                return _deleteStageCommand;
            }
        }

        private void DeleteStage(object commandParameter)
        {
            bool doesUserWantsToDeleteStage = DependencyService
             .Get<IFeedbackService>()
             .ShowQuestion("Do you really want to delete the stage? " +
             "You can't undo this");
            if (!doesUserWantsToDeleteStage)
            {
                return;
            }

            if (!EnumeratedDishStages.Remove(commandParameter as EnumeratedDishStage))
            {
                DependencyService.Get<IFeedbackService>().ShowError("Can't " +
                    "remove the stage from the dish. Try to reload page, then try to " +
                    "reload the app");
            }
            else
            {
                int stageNumber = 1;
                ObservableCollection<EnumeratedDishStage> newEnumeratedDishStages
                    = new ObservableCollection<EnumeratedDishStage>();
                foreach (EnumeratedDishStage dishStage in EnumeratedDishStages)
                {
                    dishStage.NumberOfStage = "Stage " + stageNumber++.ToString();
                    newEnumeratedDishStages.Add(dishStage);
                }
                EnumeratedDishStages = newEnumeratedDishStages;
            }
        }

        private RelayCommand _addNewStageCommand;

        public ICommand AddNewStageCommand
        {
            get
            {
                if (_addNewStageCommand == null)
                {
                    _addNewStageCommand = new RelayCommand(AddNewStage);
                }

                return _addNewStageCommand;
            }
        }

        private void AddNewStage(object commandParameter)
        {
            EnumeratedDishStages
            .Add
            (
            new EnumeratedDishStage
                (new DishStage(),
                EnumeratedDishStages.LastOrDefault() == null
                     ? "Stage 1"
                     : Convert.ToString
                       (
                        "Stage " + (GetNumberFromLastStage() + 1)
                       )
                     .ToString()
                )
            );
        }

        private int GetNumberFromLastStage()
        {
            return Convert.ToInt32(EnumeratedDishStages.Last().NumberOfStage
                .Split(' ')[1]);
        }

        private RelayCommand _addNewIngredientToStageCommand;

        public ICommand AddNewIngredientToStageCommand
        {
            get
            {
                if (_addNewIngredientToStageCommand == null)
                {
                    _addNewIngredientToStageCommand =
                        new RelayCommand(AddNewIngredientToStage);
                }

                return _addNewIngredientToStageCommand;
            }
        }

        private void AddNewIngredientToStage(object commandParameter)
        {
            EnumeratedDishStage stage = commandParameter as EnumeratedDishStage;
            ObservableCollection<EnumeratedDishStage> newDishStages =
                new ObservableCollection<EnumeratedDishStage>();
            StageIngredient newStageIngredient = new StageIngredient();
            newStageIngredient.Ingredient = Ingredients.FirstOrDefault();
            EnumeratedDishStages.First(s => s == stage).DishStage.StageIngredient
                .Add(newStageIngredient);
            foreach (EnumeratedDishStage dishStage in EnumeratedDishStages)
            {
                newDishStages.Add(dishStage);
            }
            EnumeratedDishStages = newDishStages;
        }

        private RelayCommand _addNewDishCommand;

        public ICommand AddNewDishCommand
        {
            get
            {
                if (_addNewDishCommand == null)
                {
                    _addNewDishCommand = new RelayCommand(AddNewDish,
                                                          CanAddNewDishExecuted);
                }

                return _addNewDishCommand;
            }
        }

        private bool CanAddNewDishExecuted(object arg)
        {
            StringBuilder localErrors = new StringBuilder();
            if (CurrentDish == null)
            {
                localErrors.AppendLine("Dish is unknown to the app");
                Errors = localErrors.ToString();
                return false;
            }
            if (EnumeratedDishStages == null || EnumeratedDishStages.Count == 0)
            {
                localErrors.AppendLine("Dish stages count is unknown or zero, " +
                    "you need to add at least one dish stage");
            }
            else
            {
                if (EnumeratedDishStages.Select(s => s.DishStage.ProcessDescription)
               .Any(s => string.IsNullOrWhiteSpace(s)))
                {
                    localErrors.AppendLine("You didn't input the description " +
                        "on some dish stages, though they have to be filled");
                }

                if (EnumeratedDishStages
                    .Select(s => s.DishStage.TimeInMinutes.ToString())
                    .Any(s => !(byte.TryParse(s, out _) && byte.Parse(s) > 0)))
                {
                    localErrors.AppendLine("You didn't input time in minutes " +
                        "on some dish stages, though " +
                        "they have to be filled and to be " +
                        "a positive integer number");
                }

                if (EnumeratedDishStages.Any(s => s.DishStage.StageIngredient.Count == 0))
                {
                    localErrors.AppendLine("No any ingredients are available " +
                        "in one of the stages, " +
                        "though there need to be at least one ingredient on the stage");
                }
                else
                {
                    if (EnumeratedDishStages
                        .Any(s => s.DishStage.StageIngredient.Any(si =>
                            {
                                return !(decimal.TryParse(si.Quantity.ToString(), out _)
                                     && decimal.Parse(si.Quantity.ToString()) > 0);
                            })))
                    {
                        localErrors.AppendLine("You didn't input quantity " +
                            "on some stage ingredients or " +
                            "you have filled them wrong, though " +
                            "they have to be filled and be " +
                            "a positive (probably fractional) number");
                    }
                }
            }
            if (string.IsNullOrEmpty(CurrentDish.DishName))
            {
                localErrors.AppendLine("Name is mandatory field");
            }
            if (!int.TryParse(CurrentDish.BaseServings.ToString(), out _)
                || int.Parse(CurrentDish.BaseServings.ToString()) <= 0)
            {
                localErrors.AppendLine("Base servings is a positive integer number");
            }
            if (CurrentDishCategory is null)
            {
                localErrors.AppendLine("The dish category is mandatory");
            }

            bool isValid = localErrors.Length == 0;

            Errors = localErrors.ToString();

            return isValid;
        }

        private async void AddNewDish(object commandParameter)
        {
            bool doesUserWantsToAddDish = DependencyService
              .Get<IFeedbackService>()
              .ShowQuestion("Do you really want to add a dish? You can't undo this");
            if (!doesUserWantsToAddDish)
            {
                return;
            }
            CurrentDish.FinalPriceInCents = TotalPriceInCents;
            CurrentDish.DishCategory = CurrentDishCategory;
            EnumeratedDishStages.Select(d =>
            {
                d.DishStage.StageIngredient.ToList()
                .ForEach(si => si.IngredientUnit = si.Ingredient.IngredientUnit);
                return d.DishStage;
            }).ToList().ForEach(CurrentDish.DishStage.Add);
            _ = _context.Dish.Add(CurrentDish);
            try
            {
                if (await _context.SaveChangesAsync() == 0)
                {
                    throw new DataException("Dish was not added");
                }
                _timer.Dispose();
                DependencyService.Get<IFeedbackService>().ShowInformation("A " +
                    "dish was successfully added!");
                DependencyService.Get<INavigationService>().GoBack();
            }
            catch (DataException ex)
            {
                DependencyService.Get<IFeedbackService>().ShowInformation("A " +
                 "dish was not successfully added. Try to " +
                 "reload page or try to reload the app");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        private RelayCommand _removeIngredientStageCommand;

        public ICommand RemoveIngredientStageCommand
        {
            get
            {
                if (_removeIngredientStageCommand == null)
                {
                    _removeIngredientStageCommand =
                        new RelayCommand(RemoveIngredientStage);
                }

                return _removeIngredientStageCommand;
            }
        }

        private void RemoveIngredientStage(object commandParameter)
        {
            StageIngredient ingredientStage = commandParameter as StageIngredient;
            bool doesUserWantsToDeleteIngredientStage = DependencyService
                .Get<IFeedbackService>()
                .ShowQuestion("Do you really want to delete the ingredient stage?");
            if (!doesUserWantsToDeleteIngredientStage)
            {
                return;
            }

            foreach (var enumeratedStage in EnumeratedDishStages)
            {
                _ = enumeratedStage.DishStage.StageIngredient.Remove(ingredientStage);
                EnumeratedDishStages =
                    new ObservableCollection<EnumeratedDishStage>(EnumeratedDishStages);
            }
        }

        private RelayCommand addDishImageCommand;
        private string _errors = string.Empty;

        public ICommand AddDishImageCommand
        {
            get
            {
                if (addDishImageCommand == null)
                {
                    addDishImageCommand = new RelayCommand(AddDishImage);
                }

                return addDishImageCommand;
            }
        }

        public string Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }

        private void AddDishImage(object commandParameter)
        {
            var dialogService = DependencyService.Get<OpenPhotoDialogService>();
            if (dialogService.Open())
            {
                CurrentDish.BinaryImage =
                    File.ReadAllBytes((string)dialogService.Result);
                OnPropertyChanged(nameof(CurrentDish));
                DependencyService.Get<IFeedbackService>().ShowInformation("The image " +
                    "was successfully changed!");
            }
        }
    }
}
