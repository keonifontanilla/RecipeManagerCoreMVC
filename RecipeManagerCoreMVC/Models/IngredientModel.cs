using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Models
{
    public class IngredientModel
    {
        public int Id { get; set; }
        public string Ingredient { get; set; }
        public ICollection<RecipeIngredientModel> RecipeIngredientModels { get; set; }
    }
}
