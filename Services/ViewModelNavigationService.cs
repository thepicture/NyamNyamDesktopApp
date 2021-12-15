using System;
using System.Collections.Generic;

namespace NyamNyamDesktopApp.Services
{
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
            Navigate<T>(null);
        }

        public void Navigate<T>(object obj)
        {
            if (obj == null)
            {
                NavigationJournal.Push(Activator.CreateInstance(typeof(T)));
            }
            else
            {
                NavigationJournal.Push(Activator.CreateInstance(typeof(T), obj));
            }
            SelectedNavigationTarget = NavigationJournal.Peek();
            Navigated?.Invoke();
        }
    }
}
