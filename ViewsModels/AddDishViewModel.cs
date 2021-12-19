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

        private async void PerformLoadCategoriesAndIngredients(object commandParameter)
        {
            CurrentDish = new Dish();

            DishCategories = await _context.DishCategory.ToListAsync();
            CurrentDishCategory = DishCategories.FirstOrDefault(d =>
            {
                return d.CategoryId == CurrentDish.DishCategory?.CategoryId;
            });

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

            Timer timer =
                new Timer(UpdateTotalPrice);
            if (!timer.Change(0, (int)TimeSpan.FromSeconds(1).TotalMilliseconds))
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
                    dishStage.NumberOfStage = stageNumber++.ToString();
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
                     ? "1"
                     : Convert.ToString
                       (
                         Convert.ToInt32(EnumeratedDishStages.Last().NumberOfStage) + 1
                       )
                     .ToString()
                )
            );
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
                    _addNewDishCommand = new RelayCommand(AddNewDish);
                }

                return _addNewDishCommand;
            }
        }

        private async void AddNewDish(object commandParameter)
        {
            CurrentDish.FinalPriceInCents = TotalPriceInCents;
            CurrentDish.DishCategory = CurrentDishCategory;
            _ = _context.Dish.Add(CurrentDish);
            try
            {
                if (await _context.SaveChangesAsync() == 0)
                {
                    throw new DataException("Dish was not added");
                }
                DependencyService.Get<IFeedbackService>().ShowInformation("The " +
                    "dish was successfully added!");
                DependencyService.Get<INavigationService>().GoBack();
            }
            catch (DataException ex)
            {
                DependencyService.Get<IFeedbackService>().ShowInformation("The " +
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

            foreach (var enumeratedStage in EnumeratedDishStages)
            {
                _ = enumeratedStage.DishStage.StageIngredient.Remove(ingredientStage);
                EnumeratedDishStages =
                    new ObservableCollection<EnumeratedDishStage>(EnumeratedDishStages);
            }
        }

        private RelayCommand addDishImageCommand;

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
