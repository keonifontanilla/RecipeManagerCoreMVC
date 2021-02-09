﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArticlesController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<ArticleModel> articles = _db.Articles;

            return View(articles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticlesCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                var articleModel = new ArticleModel
                {
                    ArticleTitle = model.ArticleModel.ArticleTitle,
                    ArticleBody = model.ArticleModel.ArticleBody,
                    AuthorId = userId
                };

                string FileName = null;
                if (model.Photo != null)
                {
                    var imageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "uploadedImages");
                    FileName = $"{Guid.NewGuid()}_{model.Photo.FileName}";
                    var path = Path.Combine(imageFolder, FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        model.Photo.CopyTo(fileStream);
                    }
                    articleModel.PhotoPath = FileName;
                }

                _db.Add(articleModel);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet("Articles/Article/{id?}/{name?}")]
        [AllowAnonymous]
        public IActionResult Details(int? id, string name)
        {
            ArticleModel articleModel = _db.Articles
                .Include(x => x.Author)
                .FirstOrDefault(x => x.Id == id);

            if (articleModel == null) return ErrorStatusCode404(id);

            return View(articleModel);
        }

        private IActionResult ErrorStatusCode404(int? id)
        {
            try
            {
                Response.StatusCode = 404;
                return View("ArticleNotFound", id.Value);
            }
            catch (Exception)
            {
                return Error();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
