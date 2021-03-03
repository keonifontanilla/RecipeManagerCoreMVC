using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeManagerCoreMVC.Data;
using RecipeManagerCoreMVC.Models;
using RecipeManagerCoreMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Controllers
{
    [Authorize(Policy = "IsAdmin")]
    public class AdministrationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public AdministrationController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {model.Id} cannot be found";
                return View("NotFound");
            }

            if (model.UserName != user.UserName) user.UserName = model.UserName;
            if (model.FirstName != user.FirstName) user.FirstName = model.FirstName;
            if (model.LastName != user.LastName) user.LastName = model.LastName;
            if (model.AboutMe != user.AboutMe) user.AboutMe = model.AboutMe;
            if (model.Location != user.Location) user.Location = model.Location;
            if (model.PinterestLink != user.PinterestLink) user.PinterestLink = model.PinterestLink;
            if (model.FacebookLink != user.FacebookLink) user.FacebookLink = model.FacebookLink;
            if (model.TwitterLink != user.TwitterLink) user.TwitterLink = model.TwitterLink;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return RedirectToAction("ListUsers");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded) return RedirectToAction("ListUsers");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("ListUsers");
        }

        public IActionResult ListIngredients()
        {
            IEnumerable<IngredientModel> ingredientModels = _db.Ingredients;

            return View(ingredientModels);
        }
    }
}
