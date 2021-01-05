using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            List<RecipeModel> recipes = _db.Recipes
                .OrderBy(x => Guid.NewGuid())
                .Include(x => x.RecipeInfoModel)
                .Take(3).ToList();

            return View(recipes);
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

                var newRecipe = new RecipeModel
                {
                    RecipeName = recipeModel.RecipeName,
                    RecipeDescription = recipeModel.RecipeDescription,
                    RecipeType = recipeModel.RecipeType,
                    CreatedDate = recipeModel.CreatedDate,
                    RecipeIngredientModels = recipeIngredientsModels,
                    InstructionModels = instructionModels,
                    RecipeInfoModel = recipeModel.RecipeInfoModel
                };

                _db.Add(newRecipe);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

           return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
