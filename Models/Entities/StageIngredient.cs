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
    
    public partial class StageIngredient
    {
        public int StageIngredientId { get; set; }
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public int UnitId { get; set; }
        public int DishStageId { get; set; }
    
        public virtual DishStage DishStage { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public virtual IngredientUnit IngredientUnit { get; set; }
    }
}