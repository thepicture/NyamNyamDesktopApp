using NyamNyamDesktopApp.Models.Entities;

namespace NyamNyamDesktopApp.Models
{
    public class EnumeratedDishStage
    {
        public EnumeratedDishStage(DishStage dishStage,
                                        string numberOfStage)
        {
            DishStage = dishStage;
            NumberOfStage = numberOfStage;
        }

        public DishStage DishStage { get; }
        public string NumberOfStage { get; set; }
    }
}
