using System;
using System.Collections.Generic;
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
        public string IngredientQuantity { get; set; }
        public string IngredientUnit { get; set; }
    }
}
