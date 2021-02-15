using RecipeManagerCoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<RecipeModel> FeaturedRecipes { get; set; } = new List<RecipeModel>();
        public List<RecipeModel> Favorites { get; set; } = new List<RecipeModel>();
        public List<ArticleModel> ArticleModels { get; set; } = new List<ArticleModel>();
    }
}
