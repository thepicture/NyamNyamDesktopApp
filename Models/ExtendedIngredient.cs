namespace NyamNyamDesktopApp.Models
{
    public class ExtendedIngredient
    {
        public bool IsAvailable { get; }
        public string Name { get; }
        public decimal Quantity { get; }
        public string Unit { get; }
        public int Cost { get; }

        public ExtendedIngredient(bool isAvailable, string name, decimal quantity, string unit, int cost)
        {
            IsAvailable = isAvailable;
            Name = name;
            Quantity = quantity < 0 ? 0 : quantity;
            Unit = unit;
            Cost = cost;
        }

    }
}
