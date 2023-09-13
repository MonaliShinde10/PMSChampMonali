using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

using ProductManagement.Data.Repositories;
using ProductManagement.Data;
using ProductManagement.Models.DomainModel;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestProduct
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(_options);
            _repository = new ProductRepository(_context);

            _context.Database.EnsureCreated();
        }


        [Fact]
        public void AddProduct_ShouldAddProductToDatabase()
        {
            var product = new ProductModel
            {
                Name = "Test Product",
                Price = 10000
            };

            _repository.AddProduct(product);

            var addedProduct = _context.Products.Find(product.Id);
            Assert.NotNull(addedProduct);
            Assert.Equal(product.Name, addedProduct.Name);
            Assert.Equal(product.Price, addedProduct.Price);
        }

        [Fact]
        public void UpdateProduct_ShouldUpdateExistingProduct()
        {
            var originalProduct = new ProductModel
            {
                Name = "Original Product",
                Price = 20000
            };

            _repository.AddProduct(originalProduct);

            var updatedProduct = new ProductModel
            {
                Id = originalProduct.Id,
                Name = "Updated Product",
                Price = 250000
            };

            _repository.UpdateProduct(updatedProduct);

            var productInDatabase = _context.Products.Find(originalProduct.Id);
            Assert.NotNull(productInDatabase);
            Assert.Equal(updatedProduct.Name, productInDatabase.Name);
            Assert.Equal(updatedProduct.Price, productInDatabase.Price);
        }

        [Fact]
        public void DeleteProduct_ShouldRemoveProductFromDatabase()
        {
            var product = new ProductModel
            {
                Name = "Test Product",
                Price = 10000
            };

            _repository.AddProduct(product);
            _repository.DeleteProduct(product.Id);

            var productInDatabase = _context.Products.Find(product.Id);
            Assert.Null(productInDatabase);
        }

        [Fact]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            var productsToAdd = new List<ProductModel>
            {
                new ProductModel { Name = "Product 1", Price = 10000 },
                new ProductModel { Name = "Product 2", Price = 1500 },
                new ProductModel { Name = "Product 3", Price = 2000 }
            };

            foreach (var product in productsToAdd)
            {
                _repository.AddProduct(product);
            }

            var allProducts = _repository.GetAllProducts();
            Assert.NotNull(allProducts);
            Assert.Equal(productsToAdd.Count, allProducts.Count);
        }

        [Fact]
        public void AllProducts_ShouldReturnAllProducts()
        {
            var productsToAdd = new List<ProductModel>
            {
                new ProductModel { Name = "Product 1", Price = 10003 },
                new ProductModel { Name = "Product 2", Price = 1348 },
                new ProductModel { Name = "Product 3", Price = 2099 }
            };

            foreach (var product in productsToAdd)
            {
                _repository.AddProduct(product);
            }

            var allProducts = _repository.AllProducts();
            Assert.NotNull(allProducts);
            Assert.Equal(productsToAdd.Count, allProducts.Count);
        }

        [Fact]
        public void GetProductById_ShouldReturnProduct()
        {
            var productToAdd = new ProductModel
            {
                Name = "Test Product",
                Price = 1000
            };

            _repository.AddProduct(productToAdd);
            var retrievedProduct = _repository.GetProductById(productToAdd.Id);
            Assert.NotNull(retrievedProduct);
            Assert.Equal(productToAdd.Name, retrievedProduct.Name);
            Assert.Equal(productToAdd.Price, retrievedProduct.Price);
        }

        public void Dispose()
        {
            // Clean up and dispose of the in-memory database
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
