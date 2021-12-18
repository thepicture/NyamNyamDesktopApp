using NyamNyamDesktopApp.Commands;
using NyamNyamDesktopApp.Services;
using System.Windows.Input;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class NavigationViewModel : ViewModelBase
    {
        public object SelectedViewModel =>
            DependencyService.Get<INavigationService>().SelectedNavigationTarget;
        public NavigationViewModel()
        {
            DependencyService.Get<INavigationService>().Navigated += OnNavigated;
        }

        /// <summary>
        /// Occurs when the navigation state 
        /// between <see cref="INavigationService"/>'s 
        /// navigation objects has changed.
        /// </summary>
        private void OnNavigated()
        {
            OnPropertyChanged(nameof(SelectedViewModel));
        }

        private RelayCommand _navigateToDishes;

        public ICommand NavigateToDishes
        {
            get
            {
                if (_navigateToDishes == null)
                {
                    _navigateToDishes =
                        new RelayCommand(PerformNavigateToDishes);
                }

                return _navigateToDishes;
            }
        }

        private void PerformNavigateToDishes(object commandParameter)
        {
            DependencyService.Get<INavigationService>().Navigate<DishViewModel>();
        }

        private RelayCommand _navigateToIngredients;

        public ICommand NavigateToIngredients
        {
            get
            {
                if (_navigateToIngredients == null)
                {
                    _navigateToIngredients =
                        new RelayCommand(PerformNavigateToIngredients);
                }

                return _navigateToIngredients;
            }
        }

        private void PerformNavigateToIngredients(object commandParameter)
        {
            DependencyService.Get<INavigationService>().Navigate<IngredientViewModel>();
        }
    }
}
