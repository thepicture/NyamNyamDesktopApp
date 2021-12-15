using System.ComponentModel;
using System.Runtime.CompilerServices;

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
    }
}
