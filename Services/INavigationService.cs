using System;
using System.Collections.Generic;

namespace NyamNyamDesktopApp.Services
{
    /// <summary>
    /// Defines methods for navigating.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Determines the navigation stack.
        /// </summary>
        Stack<object> NavigationJournal { get; }
        /// <summary>
        /// Fires after every call to interface methods in implementations.
        /// </summary>
        event Action Navigated;
        /// <summary>
        /// Determines the peek of the navigation stack.
        /// </summary>
        object SelectedNavigationTarget { get; }
        /// <summary>
        /// Navigates to the given type.
        /// </summary>
        /// <typeparam name="T">The type to navigate to.</typeparam>
        void Navigate<T>();
        /// <summary>
        /// Navigates to the given type with the given constructor argument.
        /// </summary>
        /// <typeparam name="T">The type to navigate to.</typeparam>
        /// <param name="param">The constructor argument.</param>
        void NavigateWithParameters<T>(object param);
        /// <summary>
        /// Pops the peek of the navigation stack.
        /// </summary>
        void GoBack();
    }
}
