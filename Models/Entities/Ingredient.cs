//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NyamNyamDesktopApp.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ingredient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ingredient()
        {
            this.StageIngredient = new HashSet<StageIngredient>();
        }
    
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int PricePerUnitInCents { get; set; }
        public int UnitId { get; set; }
        public byte CountInStock { get; set; }
    
        public virtual IngredientUnit IngredientUnit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StageIngredient> StageIngredient { get; set; }
    }
}
