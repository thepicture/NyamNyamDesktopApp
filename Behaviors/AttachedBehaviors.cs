using System.Windows;
using System.Windows.Input;

namespace NyamNyamDesktopApp.Behaviors
{
    public static class AttachedBehaviors
    {
        public static DependencyProperty LoadedCommandProperty
         = DependencyProperty.RegisterAttached(
              "LoadedCommand",
              typeof(ICommand),
              typeof(AttachedBehaviors),
              new PropertyMetadata(null, OnLoadedCommandChanged));

        private static void OnLoadedCommandChanged
             (DependencyObject dependencyObject,
              DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
            if (frameworkElement != null || e.NewValue is ICommand)
            {
                frameworkElement.Loaded += (sender, args) =>
                {
                    GetLoadedCommand(dependencyObject).Execute(null);
                };
            }
        }

        public static ICommand GetLoadedCommand(DependencyObject dependencyObject)
        {
            return (ICommand)dependencyObject.GetValue(LoadedCommandProperty);
        }

        public static void SetLoadedCommand(DependencyObject dependencyObject,
                                            ICommand newValue)
        {
            dependencyObject.SetValue(LoadedCommandProperty, newValue);
        }

    }
}
