using RecipeManagerCoreMVC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "About me")]
        public string AboutMe { get; set; }
        public string Location { get; set; }
        [Display(Name = "Pinterest")]
        public string PinterestLink { get; set; }
        [Display(Name = "Facebook")]
        public string FacebookLink { get; set; }
        [Display(Name = "Twitter")]
        public string TwitterLink { get; set; }
    }
}
