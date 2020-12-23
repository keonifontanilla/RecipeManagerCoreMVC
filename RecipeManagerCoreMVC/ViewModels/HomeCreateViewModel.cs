using RecipeManagerCoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.ViewModels
{
    public class HomeCreateViewModel
    {
        public RecipeModel RecipeModel { get; set; }
        public List<RecipeIngredientModel> RecipeIngredientModels { get; set; }
        public List<IngredientModel> IngredientModels { get; set; }
        public List<InstructionModel> InstructionModels { get; set; }
    }
}
