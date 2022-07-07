using NyamNyamDesktopApp.Models.Entities;
using NyamNyamDesktopApp.Models.Factories;
using NyamNyamDesktopApp.Services;
using NyamNyamDesktopApp.ViewsModels;
using System;
using System.Linq;
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
            if (e.Args.Contains("-import"))
            {
                MessageBoxResult isShouldContinueResult = MessageBox.Show("The app wants to import files in the database. Continue?",
                                                                          "Question",
                                                                          MessageBoxButton.YesNo,
                                                                          MessageBoxImage.Question);
                if (isShouldContinueResult == MessageBoxResult.Yes)
                {
                    ImportService.ImportIngredients();
                    ImportService.ImportDishes();
                    ImportService.ImportStages();
                    Environment.Exit(0);
                    return;
                }
            }

            base.OnStartup(e);

            DependencyService.Register<ViewModelNavigationService>();
            DependencyService.Register<NyamNyamBaseEntities>();
            DependencyService.Register<NyamNyamContextFactory>();
            DependencyService.Register<MessageBoxFeedbackService>();
            DependencyService.Register<OpenPhotoDialogService>();

            DependencyService.Get<INavigationService>().Navigate<DishViewModel>();

            NavigationView view = new NavigationView();
            view.Show();
        }
    }
}
