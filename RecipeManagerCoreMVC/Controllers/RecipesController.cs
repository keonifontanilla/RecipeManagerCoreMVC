﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecipeManagerCoreMVC.Data;
using RecipeManagerCoreMVC.Models;
using RecipeManagerCoreMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static RecipeManagerCoreMVC.Models.Enums;

namespace RecipeManagerCoreMVC.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecipesController(ILogger<HomeController> logger, ApplicationDbContext db, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index(RecipeType? recipeType)
        {
            IEnumerable<RecipeModel> recipes = _db.Recipes
                .Include(x => x.RecipeInfoModel)
                .Include(x => x.Author);

            if (recipeType != null && recipeType != RecipeType.All)
            {
                recipes = _db.Recipes
                    .Where(x => x.RecipeType == recipeType)
                    .Include(x => x.RecipeInfoModel)
                    .Include(x => x.Author);
                return View(recipes);
            }

            return View(recipes);
        }

        [HttpGet("Recipes/Recipe/{id?}/{name?}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            RecipeModel recipe = GetRecipe(id);
            
            if (recipe == null) return ErrorStatusCode404(id);
            ViewBag.FavoriteSaved = false;

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var favorites = _db.UserFavoriteRecipes.Where(x => x.UserId == user.Id);

                foreach (var favorite in favorites)
                {
                    if (favorite.RecipeId == id) ViewBag.FavoriteSaved = true;
                }
            }

            return View(recipe);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            RecipeModel recipe = GetRecipe(id);

            if (recipe == null) return ErrorStatusCode404(id);

            var recipesEditViewModel = new RecipesEditViewModel
            {
                RecipeModel = recipe
            };
            
            return View(recipesEditViewModel);
        }

        private IActionResult ErrorStatusCode404(int? id)
        {
            try
            {
                Response.StatusCode = 404;
                return View("RecipeNotFound", id.Value);
            }
            catch (Exception)
            {
                return Error();
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RecipesEditViewModel recipesEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var recipeModel = recipesEditViewModel.RecipeModel;
                // Check for existing ingredient
                if (recipeModel.RecipeIngredientModels != null)
                {
                    foreach (var recipeIngredientModel in recipeModel.RecipeIngredientModels)
                    {
                        IngredientModel existingIngredient = _db.Ingredients
                            .FirstOrDefault(x => x.Ingredient == recipeIngredientModel.IngredientsModel.Ingredient);
                        if (existingIngredient != null) recipeIngredientModel.IngredientsModel = existingIngredient;
                    }
                }

                RecipeModel recipe = GetRecipe(recipeModel.Id);
                recipe.RecipeName = recipeModel.RecipeName;
                recipe.RecipeDescription = recipeModel.RecipeDescription;
                recipe.RecipeType = recipeModel.RecipeType;
                recipe.RecipeIngredientModels = recipeModel.RecipeIngredientModels;
                recipe.InstructionModels = recipeModel.InstructionModels;
                recipe.UpdatedDated = DateTime.Now;

                string FileName = null;
                if (recipesEditViewModel.Photo != null)
                {
                    if (recipeModel.RecipeInfoModel.PhotoPath != null)
                    {
                        DeleteOldPhotoPath(recipeModel);
                    }

                    var imageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "uploadedImages");
                    FileName = $"{Guid.NewGuid()}_{recipesEditViewModel.Photo.FileName}";
                    var path = Path.Combine(imageFolder, FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        recipesEditViewModel.Photo.CopyTo(fileStream);
                    }
                    recipeModel.RecipeInfoModel.PhotoPath = FileName;
                }
                recipe.RecipeInfoModel = recipeModel.RecipeInfoModel;

                _db.Update(recipe);
                _db.SaveChanges();
                return RedirectToAction("Details", new { id = recipe.Id });
            }

            return View(recipesEditViewModel.RecipeModel);
        }

        private void DeleteOldPhotoPath(RecipeModel recipeModel)
        {
            var oldPhotoPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "uploadedImages",
                recipeModel.RecipeInfoModel.PhotoPath);
            System.IO.File.Delete(oldPhotoPath);
        }

        private RecipeModel GetRecipe(int? id)
        {
            RecipeModel recipe = _db.Recipes
                .Include(x => x.RecipeIngredientModels)
                .ThenInclude(x => x.IngredientsModel)
                .Include(x => x.InstructionModels)
                .Include(x => x.RecipeInfoModel)
                .Include(x => x.Author)
                .FirstOrDefault(x => x.Id == id);

            return recipe;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            RecipeModel recipeModel = GetRecipe(id);

            if (recipeModel == null) return ErrorStatusCode404(id);

            //// Remove ingredients that belongs to the recipe
            //foreach (var recipeIngredientModels in recipeModel.RecipeIngredientModels)
            //{
            //    _db.Remove(recipeIngredientModels.IngredientsModel);
            //}

            if (recipeModel.RecipeInfoModel != null && recipeModel.RecipeInfoModel.PhotoPath != null)
            {
                DeleteOldPhotoPath(recipeModel);
            }

            _db.Remove(recipeModel);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Search(string name)
        {
            IEnumerable<RecipeModel> recipes = _db.Recipes
                .Where(x => x.RecipeName.Contains(name))
                .Include(x => x.RecipeInfoModel)
                .Include(x => x.Author);
            return View(recipes);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Favorites(int recipeId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Unable to load user with ID '{_userManager.GetUserId(User)}'.";
                return View("NotFound");
            }

            var favoriteModel = new FavoriteModel
            {
                UserId = user.Id,
                RecipeId = recipeId
            };

            _db.UserFavoriteRecipes.Add(favoriteModel);
            _db.SaveChanges();

            return RedirectToAction("Details", new { id = recipeId });
        }
    }
}
