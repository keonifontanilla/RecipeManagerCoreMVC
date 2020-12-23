using RecipeManagerCoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.ViewModels
{
    public class HomeCreateViewModel
    {
        public RecipeIngredientModel RecipeIngredientModel { get; set; }
        public List<IngredientModel> IngredientModels { get; set; }
        public List<InstructionModel> InstructionModels { get; set; }
    }
}
