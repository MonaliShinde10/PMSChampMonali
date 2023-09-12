using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Data.Repositories;
using ProductManagement.Models.ViewModel;

namespace ProductManagement.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IProductService _productService;

        public UserController(IProductService productSevice)
        {
            _productService = productSevice;
        }

        public IActionResult ViewProducts()
        {
            var products = _productService.AllProducts();
            return View(products);
        }

        public IActionResult UserDashboard()
        {
            var userViewModel = new UserDashboardViewModel();
            return View("UserDashboard", userViewModel);
        }
    }
}
