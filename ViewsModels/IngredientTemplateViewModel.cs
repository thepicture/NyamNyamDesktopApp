using NyamNyamDesktopApp.Commands;
using NyamNyamDesktopApp.Models.Entities;
using NyamNyamDesktopApp.Models.Factories;
using NyamNyamDesktopApp.Services;
using System.Data;
using System.Windows.Input;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class IngredientTemplateViewModel : ViewModelBase
    {
        private readonly IDbContextFactory<NyamNyamBaseEntities> _dbFactory;
        private Ingredient _currentIngredient;
        private decimal _observableCountInStock;

        public IngredientTemplateViewModel(Ingredient ingredient)
        {
            _dbFactory = DependencyService.Get<NyamNyamContextFactory>();
            CurrentIngredient = ingredient;
            ObservableCountInStock = ingredient.CountInStock;
        }


        private RelayCommand _increaseNumberOfIngredientsByOneCommand;

        public ICommand IncreaseNumberOfIngredientsByOneCommand
        {
            get
            {
                if (_increaseNumberOfIngredientsByOneCommand == null)
                {
                    _increaseNumberOfIngredientsByOneCommand =
                        new RelayCommand(IncreaseNumberOfIngredientsByOne);
                }

                return _increaseNumberOfIngredientsByOneCommand;
            }
        }

        private void IncreaseNumberOfIngredientsByOne(object commandParameter)
        {
            ObservableCountInStock++;
        }

        private RelayCommand _decreaseNumberOfIngredientsByOneCommand;

        public ICommand DecreaseNumberOfIngredientsByOneCommand
        {
            get
            {
                if (_decreaseNumberOfIngredientsByOneCommand == null)
                {
                    _decreaseNumberOfIngredientsByOneCommand =
                        new RelayCommand(DecreaseNumberOfIngredientsByOne);
                }

                return _decreaseNumberOfIngredientsByOneCommand;
            }
        }

        public Ingredient CurrentIngredient
        {
            get => _currentIngredient;
            set => SetProperty(ref _currentIngredient, value);
        }
        public decimal ObservableCountInStock
        {
            get => _observableCountInStock;
            set
            {
                if (!SetProperty(ref _observableCountInStock, value))
                {
                    return;
                }
                OnPropertyChanged(nameof(IngredientTemplateViewModel));
                UpdateCountInStockInDatabase();
            }
        }

        private async void UpdateCountInStockInDatabase()
        {
            NyamNyamBaseEntities context = _dbFactory.Create();
            Ingredient databaseIngredient =
                await context.Ingredient.FindAsync(CurrentIngredient.IngredientId);

            databaseIngredient.CountInStock = ObservableCountInStock;

            try
            {
                _ = await context.SaveChangesAsync();
            }
            catch (DataException ex)
            {
                DependencyService.Get<IFeedbackService>().ShowError("Can't " +
                    "change number of ingredients. Try again or restart the app");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            CurrentIngredient = await context
                                      .Ingredient
                                      .FindAsync(CurrentIngredient.IngredientId);
        }

        private void DecreaseNumberOfIngredientsByOne(object commandParameter)
        {
            ObservableCountInStock--;
        }
    }
}
