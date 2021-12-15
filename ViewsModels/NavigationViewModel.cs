using NyamNyamDesktopApp.Services;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class NavigationViewModel : ViewModelBase
    {
        public object SelectedViewModel => DependencyService.Get<INavigationService>().SelectedNavigationTarget;
        public NavigationViewModel()
        {
            DependencyService.Get<INavigationService>().Navigated += OnNavigated;
        }

        private void OnNavigated()
        {
            OnPropertyChanged(nameof(SelectedViewModel));
        }
    }
}
