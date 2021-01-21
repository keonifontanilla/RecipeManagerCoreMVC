using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Details(string userName)
        {
            return View();
        }
    }
}
