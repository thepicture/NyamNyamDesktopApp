using NyamNyamDesktopApp.Models.Entities;
using NyamNyamDesktopApp.Services;
using NyamNyamDesktopApp.ViewsModels;
using System.Windows;

namespace NyamNyamDesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DependencyService.Register<ViewModelNavigationService>();
            DependencyService.Register<NyamNyamBaseEntities>();
            DependencyService.Register<DishDataStore>();
            DependencyService.Register<DishCategoryDataStore>();

            DependencyService.Get<INavigationService>().Navigate<DishViewModel>();

            NavigationView view = new NavigationView();
            view.Show();
        }
    }
}
