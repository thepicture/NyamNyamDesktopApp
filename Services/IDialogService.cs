namespace NyamNyamDesktopApp.Services
{
    /// <summary>
    /// Defines the method to get a result from a dialog.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// A result of a dialog and the user interaction, 
        /// if the user selected a target object, 
        /// or if no method invocation was processed, otherwise null.
        /// </summary>
        object Result { get; }
        /// <summary>
        /// Gets a result from a dialog.
        /// </summary>
        /// <returns>True if something was selected, 
        /// otherwise null.</returns>
        bool Open();
    }
}
