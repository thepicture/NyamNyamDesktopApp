using NyamNyamDesktopApp.Commands;
using NyamNyamDesktopApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace NyamNyamDesktopApp.ViewsModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        private bool _isBusy = false;

        public string Title
        {
            get => _title; set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public bool IsBusy
        {
            get => _isBusy; set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when property has changed.
        /// </summary>
        /// <param name="propertyName">A caller member name or a property name.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the value of an object's reference field.
        /// </summary>
        /// <typeparam name="T">The type of a new value.</typeparam>
        /// <param name="field">The object's reference field.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">A caller member name or a property name.</param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T field,
                                      T newValue,
                                      [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this,
                                        new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private RelayCommand _goBackCommand;

        public ICommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new RelayCommand(GoBack);
                }

                return _goBackCommand;
            }
        }

        /// <summary>
        /// Instructs <see cref="INavigationService"/> 
        /// to navigate back.
        /// </summary>
        /// <param name="commandParameter">The command parameter.</param>
        private void GoBack(object commandParameter)
        {
            DependencyService.Get<INavigationService>().GoBack();
        }
    }
}
