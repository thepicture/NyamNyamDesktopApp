using System.Windows;

namespace NyamNyamDesktopApp.Services
{
    /// <summary>
    /// Implements methods to show feedback to an user 
    /// with the <see cref="MessageBox"/> service.
    /// </summary>
    public class MessageBoxFeedbackService : IFeedbackService
    {
        public void ShowError(string message)
        {
            _ = MessageBox.Show(message,
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }

        public void ShowInformation(string message)
        {
            _ = MessageBox.Show(message,
                             "Information",
                             MessageBoxButton.OK,
                             MessageBoxImage.Information);
        }

        public bool ShowQuestion(string message)
        {
            return MessageBox.Show(message,
                           "Question",
                           MessageBoxButton.YesNo,
                           MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void ShowWarning(string message)
        {
            _ = MessageBox.Show(message,
                           "Warning",
                           MessageBoxButton.OK,
                           MessageBoxImage.Warning);
        }
    }
}
