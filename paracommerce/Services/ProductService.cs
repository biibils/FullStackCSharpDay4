using paracommerce.Models;

namespace paracommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products;

        public ProductService(List<Product>? products = null)
        {
            _products = products ?? new List<Product>();
        }

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public Product? GetProductById(int id)
        {
            if (!_products.Any())
            {
                return null;
            }
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            if (product == null)
                return;

            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            if (!_products.Any() || product == null)
                return;

            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
            }
        }

        public void DeleteProduct(int id)
        {
            if (!_products.Any())
                return;

            _products.RemoveAll(p => p.Id == id);
        }
    }
}
