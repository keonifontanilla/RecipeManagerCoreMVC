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
        public InstructionModel InstructionModel { get; set; }
    }
}
