using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecipeManagerCoreMVC.Data;
using RecipeManagerCoreMVC.Models;
using RecipeManagerCoreMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static RecipeManagerCoreMVC.Models.Enums;

namespace RecipeManagerCoreMVC.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public RecipesController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(RecipeType? recipeType)
        {
            IEnumerable<RecipeModel> recipes = _db.Recipes;

            if (recipeType != null && recipeType != RecipeType.All)
            {
                recipes = _db.Recipes.Where(x => x.RecipeType == recipeType);
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

            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RecipeModel recipeModel)
        {
            if (ModelState.IsValid)
            {                
                // Check for existing ingredient
                if (recipeModel.RecipeIngredientModels != null)
                {
                    foreach (var recipeIngredientModel in recipeModel.RecipeIngredientModels)
                    {
                        IngredientModel existingIngredient = _db.Ingredients.FirstOrDefault(x => x.Ingredient == recipeIngredientModel.IngredientsModel.Ingredient);
                        if (existingIngredient != null) recipeIngredientModel.IngredientsModel = existingIngredient;
                    }
                }

                RecipeModel recipe = GetRecipe(recipeModel.Id);
                recipe.RecipeName = recipeModel.RecipeName;
                recipe.RecipeDescription = recipeModel.RecipeDescription;
                recipe.RecipeType = recipeModel.RecipeType;
                recipe.RecipeIngredientModels = recipeModel.RecipeIngredientModels;
                recipe.InstructionModels = recipeModel.InstructionModels;
                recipe.RecipeInfoModel = recipeModel.RecipeInfoModel;
                _db.Update(recipe);

                _db.SaveChanges();
                return RedirectToAction("Details", new { id = recipe.Id });
            }

            return View(recipeModel);
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
            RecipeModel recipeModel = _db.Recipes
                .Include(x => x.RecipeIngredientModels)
                .ThenInclude(x => x.IngredientsModel)
                .FirstOrDefault(x => x.Id == id);

            if (recipeModel == null) return Error();

            //// Remove ingredients that belongs to the recipe
            //foreach (var recipeIngredientModels in recipeModel.RecipeIngredientModels)
            //{
            //    _db.Remove(recipeIngredientModels.IngredientsModel);
            //}

            _db.Remove(recipeModel);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search(string name)
        {
            IEnumerable<RecipeModel> recipes = _db.Recipes.Where(x => x.RecipeName.Contains(name));
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
