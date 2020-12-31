using Microsoft.AspNetCore.Http;
using RecipeManagerCoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.ViewModels
{
    public class RecipesEditViewModel
    {
        public RecipeModel RecipeModel { get; set; }
        public IFormFile Photo { get; set; }
    }
}
