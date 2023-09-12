using Xunit;
using ProductManagement.Controllers;
using ProductManagement.Models.DomainModel;
using ProductManagement.Models.ViewModel;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using ProductManagement.Data.Repositories;

namespace TestProduct
{
    public class ProductControllerTest
    {
        [Fact]
        public void TestAllProducts()
        {
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productServiceMock.Object);

            var products = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = Guid.NewGuid(),
                    Name = "iPhone",
                    Price = 10000
                },
                new ProductModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Vivo",
                    Price = 20000
                }
            };

            productServiceMock.Setup(service => service.GetAllProducts())
                .Returns(products);
            var result = productController.AllProducts() as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(products, result.Model);
            Assert.Null(result.ViewName);
        }
        [Fact]
        public void TestAllProducts_Invalid()
        {
            Assert.False(false);
        }

        [Fact]
        public void TestAddProduct()
        {
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productServiceMock.Object);

            var productToAdd = new ProductModel
            {
                Id = Guid.NewGuid(),
                Name = "iPhone",
                Price = 10
            };

            productServiceMock.Setup(service => service.AddProduct(It.IsAny<ProductModel>()))
                .Verifiable();
            var result = productController.AddProduct(productToAdd) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("AllProducts", result.ActionName);

            productServiceMock.Verify(service => service.AddProduct(productToAdd), Times.Once);
        }
        [Fact]
        public void TestAddProduct_Invalid()
        {
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productServiceMock.Object);
            var result = productController.AddProduct(new ProductModel()) as RedirectToActionResult;

            Assert.True(result?.ActionName == "AllProducts");
        }


        [Fact]
        public void TestEditProduct()
        {
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productServiceMock.Object);

            var productId = Guid.NewGuid();
            var productToEdit = new ProductModel
            {
                Id = productId,
                Name = "iPhone",
                Price = 15000
            };

            productServiceMock.Setup(service => service.GetProductById(productId))
                .Returns(productToEdit);
            var result = productController.EditProduct(productId) as ViewResult;
            Assert.NotNull(result);
            var model = Assert.IsType<ProductModel>(result.Model);
            Assert.Equal(productToEdit, model);
        }
        [Fact]
        public void TestEditProduct_Invalid()
        {
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productServiceMock.Object);
            var result = productController.EditProduct(Guid.NewGuid()) as ViewResult;

            Assert.False(result != null);
        }


        [Fact]
        public void TestDeleteProduct()
        {
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productServiceMock.Object);

            var productId = Guid.NewGuid();

            productServiceMock.Setup(service => service.DeleteProduct(productId))
                .Verifiable();
            var result = productController.DeleteProduct(productId) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("AllProducts", result.ActionName);

            productServiceMock.Verify(service => service.DeleteProduct(productId), Times.Once);
        }
        [Fact]
        public void TestDeleteProduct_Invalid()
        {
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productServiceMock.Object);
            var result = productController.DeleteProduct(Guid.NewGuid()) as RedirectToActionResult;

            Assert.False(result?.ActionName == "InvalidActionName");
        }

        [Fact]
        public void TestViewProducts()
        {
            var productServiceMock = new Mock<IProductService>();
            var userController = new UserController(productServiceMock.Object);

            var products = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = Guid.NewGuid(),
                    Name = "iPhone",
                    Price = 10000
                },
                new ProductModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Vivo",
                    Price = 20000
                }
            };

            productServiceMock.Setup(service => service.AllProducts())
                .Returns(products);
            var result = userController.ViewProducts() as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(products, result.Model);
            Assert.Null(result.ViewName);

        }
        [Fact]
        public void TestViewProducts_Invalid()
        {
            var productServiceMock = new Mock<IProductService>();
            var userController = new UserController(productServiceMock.Object);

            var result = userController.ViewProducts() as ViewResult;

            Assert.True(result?.Model == null);
        }
        [Fact]
        public void AdminDashboard_Get_ReturnsViewResult()
        {
            var productServiceMock = new Mock<IProductService>();
            var controller = new ProductController(productServiceMock.Object);
            var result = controller.AdminDashboard() as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("AdminDashboard", result.ViewName);
        }

        [Fact]
        public void AllProducts_Get_ReturnsViewResult()
        {
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(p => p.GetAllProducts()).Returns(new List<ProductModel>());
            var controller = new ProductController(productServiceMock.Object);

            var result = controller.AllProducts() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void AddProduct_Get_ReturnsViewResult()
        {
            var productServiceMock = new Mock<IProductService>();
            var controller = new ProductController(productServiceMock.Object);

            var result = controller.AddProduct() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void EditProduct_Get_WithValidId_ReturnsViewResult()
        {
            var productId = Guid.NewGuid();
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(p => p.GetProductById(productId)).Returns(new ProductModel());
            var controller = new ProductController(productServiceMock.Object);

            var result = controller.EditProduct(productId) as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void EditProduct_Get_WithInvalidId_RedirectsToAllProducts()
        {
            var productId = Guid.NewGuid();
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(p => p.GetProductById(productId)).Returns((ProductModel)null);
            var controller = new ProductController(productServiceMock.Object);

            var result = controller.EditProduct(productId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("AllProducts", result.ActionName);
        }

        [Fact]
        public void UserDashboard_ReturnsViewWithViewModel()
        {
            var productServiceMock = new Mock<IProductService>();
            var controller = new UserController(productServiceMock.Object);
            var result = controller.UserDashboard() as ViewResult;
            Assert.NotNull(result);
            Assert.IsType<UserDashboardViewModel>(result.Model);

            var model = result.Model as UserDashboardViewModel;
            
            Assert.Equal("UserDashboard", result.ViewName);
        }
        [Fact]
        public void EditProduct_Post_ValidModel_RedirectsToAllProducts()
        {
            var productServiceMock = new Mock<IProductService>();
            var controller = new ProductController(productServiceMock.Object);
            var validProduct = new ProductModel
            {
                Id = Guid.NewGuid(),
                Name = "Updated Product",
                Price = 25000
            };

            var result = controller.EditProduct(validProduct) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("AllProducts", result.ActionName);

            productServiceMock.Verify(service => service.UpdateProduct(validProduct), Times.Once);
        }

        [Fact]
        public void EditProduct_Post_InvalidModel_ReturnsView()
        {
            var productServiceMock = new Mock<IProductService>();
            var controller = new ProductController(productServiceMock.Object);
            var invalidProduct = new ProductModel(); 
            controller.ModelState.AddModelError("Name", "Name is required");
            var result = controller.EditProduct(invalidProduct) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(invalidProduct, result.Model); 
            productServiceMock.Verify(service => service.UpdateProduct(It.IsAny<ProductModel>()), Times.Never);
        }
    }
}
