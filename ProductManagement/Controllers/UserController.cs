using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Data.Repositories;
using ProductManagement.Models.DomainModel;
using ProductManagement.Models.ViewModel;
using System;
using System.Collections.Generic;

namespace ProductManagement.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IProductService _productService;

        public UserController(IProductService productService)
        {
            _productService = productService;
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

        public IActionResult ViewCart()
        {
            List<ProductModel> cartItems = GetCartItems();
            return View(cartItems);
        }

        public IActionResult AddToCart(Guid productId)
        {
            var product = _productService.GetProductById(productId);
            if (product != null)
            {
                AddItemToCart(product);
                return View(product); 
            }
            return RedirectToAction("ViewProducts");
        }


        private void AddItemToCart(ProductModel product)
        {
            var cart = HttpContext.Session.Get<List<ProductModel>>("Cart") ?? new List<ProductModel>();
            cart.Add(product);
            HttpContext.Session.Set("Cart", cart);
        }

        private List<ProductModel> GetCartItems()
        {
            return HttpContext.Session.Get<List<ProductModel>>("Cart") ?? new List<ProductModel>();
        }
    }
}
