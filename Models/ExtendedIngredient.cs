using NyamNyamDesktopApp.Models.Entities;

namespace NyamNyamDesktopApp.Models
{
    public class ExtendedIngredient
    {
        public ExtendedIngredient(Ingredient ingredient, bool isAvailable)
        {
            Ingredient = ingredient;
            IsAvailable = isAvailable;
        }

        public Ingredient Ingredient { get; }
        public bool IsAvailable { get; }
    }
}
