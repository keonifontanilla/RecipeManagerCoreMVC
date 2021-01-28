using Microsoft.AspNetCore.Identity;
using RecipeManagerCoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string AboutMe { get; set; }
        [PersonalData]
        public string Location { get; set; }
        [PersonalData]
        public string PinterestLink { get; set; }
        [PersonalData]
        public string FacebookLink { get; set; }
        [PersonalData]
        public string TwitterLink { get; set; }
        public ICollection<RecipeModel> RecipeModels { get; set; }
        public ICollection<FavoriteModel> FavoriteModels { get; set; }
    }
}
