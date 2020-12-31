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
using static RecipeManagerCoreMVC.Models.Enums;

namespace RecipeManagerCoreMVC.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RecipesController(ILogger<HomeController> logger, ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index(RecipeType? recipeType)
        {
            IEnumerable<RecipeModel> recipes = _db.Recipes.Include(x => x.RecipeInfoModel);

            if (recipeType != null && recipeType != RecipeType.All)
            {
                recipes = _db.Recipes.Where(x => x.RecipeType == recipeType).Include(x => x.RecipeInfoModel);
                return View(recipes);
            }

            return View(recipes);
        }

        [HttpGet("Recipes/Recipe/{id?}/{name?}")]
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0) return Error();
            RecipeModel recipe = GetRecipe(id);

            return View(recipe);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return Error();
            RecipeModel recipe = GetRecipe(id);
            var recipesEditViewModel = new RecipesEditViewModel
            {
                RecipeModel = recipe
            };
            
            return View(recipesEditViewModel);
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

                string FileName = null;
                if (recipesEditViewModel.Photo != null)
                {
                    if (recipeModel.RecipeInfoModel.PhotoPath != null)
                    {
                        var oldPhotoPath = Path.Combine(_hostingEnvironment.WebRootPath, "images",
                            recipeModel.RecipeInfoModel.PhotoPath);
                        System.IO.File.Delete(oldPhotoPath);
                    }

                    var imageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
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

        private RecipeModel GetRecipe(int? id)
        {
            RecipeModel recipe = _db.Recipes
                .Include(x => x.RecipeIngredientModels)
                .ThenInclude(x => x.IngredientsModel)
                .Include(x => x.InstructionModels)
                .Include(x => x.RecipeInfoModel)
                .FirstOrDefault(x => x.Id == id);

            return recipe;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            RecipeModel recipeModel = GetRecipe(id);

            if (recipeModel == null) return Error();

            //// Remove ingredients that belongs to the recipe
            //foreach (var recipeIngredientModels in recipeModel.RecipeIngredientModels)
            //{
            //    _db.Remove(recipeIngredientModels.IngredientsModel);
            //}

            if (recipeModel.RecipeInfoModel != null && recipeModel.RecipeInfoModel.PhotoPath != null)
            {
                var oldPhotoPath = Path.Combine(_hostingEnvironment.WebRootPath, "images",
                    recipeModel.RecipeInfoModel.PhotoPath);
                System.IO.File.Delete(oldPhotoPath);
            }

            _db.Remove(recipeModel);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search(string name)
        {
            IEnumerable<RecipeModel> recipes = _db.Recipes
                .Where(x => x.RecipeName.Contains(name))
                .Include(x => x.RecipeInfoModel);
            return View(recipes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
