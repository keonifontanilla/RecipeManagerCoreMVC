using Microsoft.AspNetCore.Mvc;
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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<RecipeModel> recipes = _db.Recipes;

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

                var index = 0;
                foreach (var ingredientModel in ingredientModels)
                {
                    recipeIngredientsModels[index].IngredientsModel = ingredientModel;
                    index++;
                }

                var newRecipe = new RecipeModel
                {
                    RecipeName = recipeModel.RecipeName,
                    RecipeDescription = recipeModel.RecipeDescription,
                    RecipeType = recipeModel.RecipeType,
                    CreatedDate = recipeModel.CreatedDate,
                    RecipeIngredientModels = recipeIngredientsModels,
                    InstructionModels = instructionModels
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
