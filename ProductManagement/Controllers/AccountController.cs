using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProductManagement.Services;
using System.Data;

namespace ProductManagement.Controllers
{
   
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
            
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterAsync(model);

                if (result)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registration failed.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _userService.LoginAsync(model);

                if (loginResult)
                {
                    var user = await _userService.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        var roles = await _userService.GetRolesAsync(model.Email);

                        if (roles.Contains("SuperAdmin"))
                        {
                            return RedirectToAction("SuperAdminDashboard", "SuperAdmin");
                        }
                        else if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("AdminDashboard", "Product");
                        }
                        else if (roles.Contains("User"))
                        {
                            return RedirectToAction("UserDashboard", "User");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User not found.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();

            return RedirectToAction("Index", "Account");
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
       

    }
}
