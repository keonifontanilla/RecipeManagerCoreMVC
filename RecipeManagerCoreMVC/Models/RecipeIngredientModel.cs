using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Models
{
    public class RecipeIngredientModel
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public RecipeModel RecipeModel { get; set; }
        public IngredientModel IngredientsModel { get; set; }
        [DisplayName("Quantity")]
        public string IngredientQuantity { get; set; }
        [DisplayName("Unit")]
        public string IngredientUnit { get; set; }
    }
}
