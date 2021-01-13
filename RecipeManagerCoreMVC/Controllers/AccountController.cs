using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeManagerCoreMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser>  signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // Client side validation
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckDuplicateEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return Json(true);

            return Json($"Email {email} is already taken");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = registerViewModel.Email, Email = registerViewModel.Email };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                    // For confirmation link in output window
                    _logger.Log(LogLevel.Warning, confirmationLink);

                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    ViewBag.SuccessTitle = "Registration successful";
                    ViewBag.SuccessMessage = "Before you can login, please confirm your email, by clicking on the confirmation link sent to your email.";
                    return View("Success");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(registerViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User Id {userId} is invalid";
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                ViewBag.SuccessTitle = "Confirmation successful";
                ViewBag.SuccessMessage = "Thank you for confirming your email.";
                return View("Success");
            }

            ViewBag.SuccessTitle = "Email cannot be confirmed";
            ViewBag.SuccessMessage = "";
            return View("Success");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

                if (user != null && !user.EmailConfirmed && (await _userManager.CheckPasswordAsync(user, loginViewModel.Password)))
                {
                    ModelState.AddModelError("", "Email not confirmed yet");
                    return View(loginViewModel);
                }

                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else 
                    { 
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Invalid Login Attempt.");
            }

            return View(loginViewModel);
        }
    }
}
