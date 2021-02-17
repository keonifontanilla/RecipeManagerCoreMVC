using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeManagerCoreMVC.Data;
using RecipeManagerCoreMVC.Models;
using RecipeManagerCoreMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(ApplicationDbContext db, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User {userName} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;

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

        [AllowAnonymous]
        public async Task<IActionResult> Recipes(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User {userName} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;

            var recipes = _db.Recipes
                .Include(x => x.RecipeInfoModel)
                .Include(x => x.Author)
                .Where(x => x.AuthorId == user.Id);

            return View(recipes);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Favorites(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User {userName} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;

            var recipes = _db.UserFavoriteRecipes
                .Where(x => x.UserId == user.Id)
                .Include(x => x.RecipeModel)
                .ThenInclude(x => x.Author)
                .Select(x => x.RecipeModel);

            return View(recipes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFavorite(RecipeModel model, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User {userName} cannot be found";
                return View("NotFound");
            }

            var favoriteModel = new FavoriteModel
            {
                UserId = user.Id,
                RecipeId = model.Id
            };

            _db.UserFavoriteRecipes.Remove(favoriteModel);
            _db.SaveChanges();

            return RedirectToAction("Favorites", new { userName = user.UserName });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Articles(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User {userName} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;

            IEnumerable<ArticleModel> articleModels = _db.Articles
                .Where(x => x.AuthorId == user.Id)
                .Include(x => x.Author);

            return View(articleModels);
        }
    }
}
