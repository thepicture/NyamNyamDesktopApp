using System;
using System.Collections.Generic;

namespace NyamNyamDesktopApp.Services
{
    /// <summary>
    /// Implements methods for navigating through view models.
    /// </summary>
    public class ViewModelNavigationService : INavigationService
    {
        public Stack<object> NavigationJournal { get; }
        public ViewModelNavigationService()
        {
            NavigationJournal = new Stack<object>();
        }

        public object SelectedNavigationTarget { get; private set; }

        public event Action Navigated;

        public void GoBack()
        {
            _ = NavigationJournal.Pop();
            SelectedNavigationTarget = NavigationJournal.Peek();
            Navigated?.Invoke();
        }

        public void Navigate<T>()
        {
            bool isStackNotEmpty = NavigationJournal.Count > 0;
            if (isStackNotEmpty)
            {
                bool isTryingToNavigateToSameType = NavigationJournal.Peek()
                                                                .GetType() == typeof(T);
                if (isTryingToNavigateToSameType)
                {
                    return;
                }
            }
            NavigateWithParameters<T>(null);
        }

        public void NavigateWithParameters<T>(object param)
        {
            if (param == null)
            {
                NavigationJournal.Push(Activator.CreateInstance(typeof(T)));
            }
            else
            {
                NavigationJournal.Push(Activator.CreateInstance(typeof(T), param));
            }
            SelectedNavigationTarget = NavigationJournal.Peek();
            Navigated?.Invoke();
        }
    }
}
