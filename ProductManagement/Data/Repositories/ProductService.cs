using ProductManagement.Models.DomainModel;

namespace ProductManagement.Data.Repositories
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<ProductModel> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public ProductModel GetProductById(Guid id)
        {
            return _productRepository.GetProductById(id);
        }

        public void AddProduct(ProductModel product)
        {
            _productRepository.AddProduct(product);
        }

        public void UpdateProduct(ProductModel product)
        {
            _productRepository.UpdateProduct(product);
        }

        public void DeleteProduct(Guid id)
        {
            _productRepository.DeleteProduct(id);
        }
        
        public List<ProductModel> AllProducts()
        {
            return _productRepository.AllProducts();
        }
    }

}
