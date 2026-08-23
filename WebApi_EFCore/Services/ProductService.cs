using POSSystem2.Data;
using POSSystem2.DTOs.Products;
using POSSystem2.Models;
using POSSystem2.Services;
using POSSystem2.Data;
using POSSystem2.DTOs.Products;
using POSSystem2.Models;

namespace POSSystem2.Services
{
    public class ProductService : IProductService
    {
        private readonly PosDbContext _context;

        public ProductService(PosDbContext context)
        {
            _context = context;
        }

        public List<ProductDto> GetAll()
        {
            return _context.Products
                .Select(p => new ProductDto
                {
                    Sku = p.Sku,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category,
                    Stock = p.Stock
                })
                .ToList();
        }

        public ProductDto? GetBySku(string sku)
        {
            return _context.Products
                .Where(p => p.Sku == sku)
                .Select(p => new ProductDto
                {
                    Sku = p.Sku,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category,
                    Stock = p.Stock
                })
                .FirstOrDefault();
        }

        public ProductDto? Create(CreateProductDto productDto)
        {
            Product? existingProduct = _context.Products
                .FirstOrDefault(p => p.Sku == productDto.Sku);

            if (existingProduct != null)
            {
                return null;
            }

            Product product = new Product();

            product.Sku = productDto.Sku;
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Category = productDto.Category;
            product.Stock = productDto.Stock;

            _context.Products.Add(product);
            _context.SaveChanges();

            ProductDto result = new ProductDto();

            result.Sku = product.Sku;
            result.Name = product.Name;
            result.Price = product.Price;
            result.Category = product.Category;
            result.Stock = product.Stock;

            return result;
        }

        public ProductDto? Update(
            string sku,
            UpdateProductDto productDto)
        {
            Product? product = _context.Products
                .FirstOrDefault(p => p.Sku == sku);

            if (product == null)
            {
                return null;
            }

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Category = productDto.Category;
            product.Stock = productDto.Stock;

            _context.SaveChanges();

            ProductDto result = new ProductDto();

            result.Sku = product.Sku;
            result.Name = product.Name;
            result.Price = product.Price;
            result.Category = product.Category;
            result.Stock = product.Stock;

            return result;
        }

        public bool Delete(string sku)
        {
            Product? product = _context.Products
                .FirstOrDefault(p => p.Sku == sku);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return true;
        }
    }
}