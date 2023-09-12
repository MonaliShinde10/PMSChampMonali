using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Data.Repositories;
using ProductManagement.Models.DomainModel;
using ProductManagement.Models.ViewModel;
using System;

namespace ProductManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult AdminDashboard()
        {
            var adminViewModel = new AdminDashboardViewModel();
            return View("AdminDashboard", adminViewModel);
        }

        public IActionResult AllProducts()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product);
                return RedirectToAction("AllProducts");
            }
            return View(product);
        }

        public IActionResult EditProduct(Guid id)
        {
            var product = _productService.GetProductById(id);
            if (product != null)
            {
                return View(product);
            }
            return RedirectToAction("AllProducts");
        }

        [HttpPost]
        public IActionResult EditProduct(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product);
                return RedirectToAction("AllProducts");
            }
            return View(product);
        }


        public IActionResult DeleteProduct(Guid id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return RedirectToAction("AllProducts");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }

}
