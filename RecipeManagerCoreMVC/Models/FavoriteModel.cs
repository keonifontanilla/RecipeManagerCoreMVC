using RecipeManagerCoreMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Models
{
    public class FavoriteModel
    {
        public string UserId { get; set; }
        public int RecipeId { get; set; }
        public RecipeModel RecipeModel { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
