using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeManagerCoreMVC.Data;
using RecipeManagerCoreMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Details(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User {userName} cannot be found";
                return View("NotFound");
            }

            var model = new ProfileViewModel
            {
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AboutMe = user.AboutMe,
                Location = user.Location,
                PinterestLink = user.PinterestLink,
                FacebookLink = user.FacebookLink,
                TwitterLink = user.TwitterLink
            };

            return View(model);
        }
    }
}
