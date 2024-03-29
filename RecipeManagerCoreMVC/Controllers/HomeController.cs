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

namespace RecipeManagerCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<RecipeModel> featuredRecipes = _db.Recipes
                .OrderBy(x => Guid.NewGuid())
                .Include(x => x.RecipeInfoModel)
                .Include(x => x.Author)
                .Take(3).ToList();

            List<ArticleModel> articles = _db.Articles
                .OrderBy(x => Guid.NewGuid())
                .Include(x => x.Author)
                .Take(9).ToList();

            var model = new HomeIndexViewModel
            {
                FeaturedRecipes = featuredRecipes,
                ArticleModels = articles
            };

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                List<RecipeModel> favorites = _db.UserFavoriteRecipes
                .Include(x => x.RecipeModel.RecipeInfoModel)
                .Where(x => x.UserId == user.Id)
                .Select(x => x.RecipeModel).ToList();
                model.Favorites = favorites;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HomeCreateViewModel homeCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var recipeModel = homeCreateViewModel.RecipeModel;
                var ingredientModels = homeCreateViewModel.IngredientModels;
                var recipeIngredientsModels = homeCreateViewModel.RecipeIngredientModels;
                var instructionModels = homeCreateViewModel.InstructionModels;

                if (ingredientModels != null)
                {
                    var index = 0;
                    foreach (var ingredientModel in ingredientModels)
                    {
                        // Check for existing ingredient
                        IngredientModel existingIngredient =_db.Ingredients.FirstOrDefault(x => x.Ingredient == ingredientModel.Ingredient);
                        if (existingIngredient != null)
                        {
                            recipeIngredientsModels[index].IngredientId = existingIngredient.Id;
                        }
                        else
                        {
                            recipeIngredientsModels[index].IngredientsModel = ingredientModel;
                        }
                        index++;
                    }
                }

                string FileName = null;
                if (homeCreateViewModel.Photo != null)
                {
                    var imageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "uploadedImages");
                    FileName = $"{Guid.NewGuid()}_{homeCreateViewModel.Photo.FileName}";
                    var path = Path.Combine(imageFolder, FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        homeCreateViewModel.Photo.CopyTo(fileStream);
                    }
                    recipeModel.RecipeInfoModel.PhotoPath = FileName; 
                }

                var userId = _userManager.GetUserId(User);

                var newRecipe = new RecipeModel
                {
                    RecipeName = recipeModel.RecipeName,
                    RecipeDescription = recipeModel.RecipeDescription,
                    RecipeType = recipeModel.RecipeType,
                    CreatedDate = recipeModel.CreatedDate,
                    RecipeIngredientModels = recipeIngredientsModels,
                    InstructionModels = instructionModels,
                    RecipeInfoModel = recipeModel.RecipeInfoModel,
                    AuthorId = userId
                };

                _db.Add(newRecipe);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

           return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
