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
    
    public partial class Dish
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dish()
        {
            this.DishStage = new HashSet<DishStage>();
        }
    
        public int DishId { get; set; }
        public string DishName { get; set; }
        public byte BaseServings { get; set; }
        public int CategoryId { get; set; }
        public string ImagePath { get; set; }
        public string SourceRecipeLink { get; set; }
        public string Description { get; set; }
        public int FinalPriceInCents { get; set; }
    
        public virtual DishCategory DishCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DishStage> DishStage { get; set; }
    }
}
