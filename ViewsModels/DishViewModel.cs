using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NyamNyamDesktopApp.ViewsModels
{
    public class DishViewModel : ViewModelBase
    {
        private IEnumerable<Dish> _dishes;
        public DishViewModel()
        {
            Title = "Dishes";
            Task.Run(() => GetDishes());
        }

        public IEnumerable<Dish> Dishes
        {
            get => _dishes; set
            {
                _dishes = value;
                OnPropertyChanged();
            }
        }

        private async void GetDishes()
        {
            Dishes = await DishDataStore.GetAllASync();
        }
    }
}
