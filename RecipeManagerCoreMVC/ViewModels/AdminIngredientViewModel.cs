using RecipeManagerCoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.ViewModels
{
    public class AdminIngredientViewModel
    {
        public IngredientModel IngredientModel { get; set; }
        public IEnumerable<IngredientModel> IngredientModels { get; set; }
    }
}
