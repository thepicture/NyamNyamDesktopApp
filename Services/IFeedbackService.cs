namespace NyamNyamDesktopApp.Services
{
    /// <summary>
    /// Defines methods for showing feedback to an user.
    /// </summary>
    public interface IFeedbackService
    {
        /// <summary>
        /// Shows an error feedback to an user.
        /// </summary>
        /// <param name="message">A feedback message text.</param>
        void ShowError(string message);
        /// <summary>
        /// Shows an information feedback to an user.
        /// </summary>
        /// <param name="message">A feedback message text.</param>
        void ShowInformation(string message);
        /// <summary>
        /// Shows an warning feedback to an user.
        /// </summary>
        /// <param name="message">A feedback message text.</param>
        void ShowWarning(string message);
        /// <summary>
        /// Show an question to an user.
        /// </summary>
        /// <returns>True if the user accepts, otherwise false.</returns>
        /// <param name="message">A feedback message text.</param>
        bool ShowQuestion(string message);
    }
}
