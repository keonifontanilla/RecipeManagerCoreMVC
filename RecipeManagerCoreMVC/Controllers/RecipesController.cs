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

        public IActionResult Index()
        {
            IEnumerable<RecipeModel> recipes = _db.Recipes;

            return View(recipes);
        }

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

        //TODO - Handle adding new instructions or ingredients on update
        [HttpPost]
        public IActionResult Edit(RecipeModel recipeModel)
        {
            if (ModelState.IsValid)
            {
                var recipe = new RecipeModel
                {
                    Id = recipeModel.Id,
                    RecipeName = recipeModel.RecipeName,
                    RecipeDescription = recipeModel.RecipeDescription,
                    RecipeType = recipeModel.RecipeType,
                    InstructionModels = recipeModel.InstructionModels
                };
                _db.Recipes.Update(recipe);

                foreach (var recipeIngredientModel in recipeModel.RecipeIngredientModels)
                {
                    IngredientModel ingredientModel = _db.Ingredients.FirstOrDefault(x => x.Id == recipeIngredientModel.IngredientId);
                    ingredientModel.Ingredient = recipeIngredientModel.IngredientsModel.Ingredient;
                    RecipeIngredientModel recipeIngredient = _db.RecipeIngredients.FirstOrDefault(x => x.IngredientId == recipeIngredientModel.IngredientId);
                    recipeIngredient.IngredientQuantity = recipeIngredientModel.IngredientQuantity;
                    recipeIngredient.IngredientUnit = recipeIngredientModel.IngredientUnit;

                    _db.Update(recipeIngredient);
                    _db.Update(ingredientModel);
                }

                //RecipeModel recipe = GetRecipe(recipeModel.Id);
                //recipe.RecipeName = recipeModel.RecipeName;
                //recipe.RecipeDescription = recipeModel.RecipeDescription;
                //recipe.RecipeType = recipeModel.RecipeType;
                //recipe.RecipeIngredientModels = recipeModel.RecipeIngredientModels;
                //recipe.InstructionModels = recipeModel.InstructionModels;
                //_db.Update(recipe);

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recipeModel);
        }

        private RecipeModel GetRecipe(int? id)
        {
            RecipeModel recipe = _db.Recipes
                .Include(x => x.RecipeIngredientModels)
                .ThenInclude(x => x.IngredientsModel)
                .Include(x => x.InstructionModels)
                .FirstOrDefault(x => x.Id == id);

            return recipe;
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
