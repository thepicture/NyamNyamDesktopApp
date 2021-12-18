using NyamNyamDesktopApp.Commands;
using NyamNyamDesktopApp.Models.Entities;
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

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private void GoBack(object commandParameter)
        {
            DependencyService.Get<INavigationService>().GoBack();
        }
    }
}
