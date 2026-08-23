using POSSystem2.Data;
using POSSystem2.DTOs.Products;
using POSSystem2.Models;

namespace POSSystem2.Services
{
    public class ProductService : IProductService
    {
        private readonly InMemoryData _data;

        public ProductService(InMemoryData data)
        {
            _data = data;
        }

        // Get all products
        public List<ProductDto> GetAll()
        {
            List<ProductDto> products = new List<ProductDto>();

            foreach (Product product in _data.Products)
            {
                ProductDto productDto = new ProductDto();

                productDto.Sku = product.Sku;
                productDto.Name = product.Name;
                productDto.Price = product.Price;
                productDto.Category = product.Category;
                productDto.Stock = product.Stock;

                products.Add(productDto);
            }

            return products;
        }

        // Get one product by SKU
        public ProductDto? GetBySku(string sku)
        {
            Product? product = null;

            foreach (Product item in _data.Products)
            {
                if (item.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase))
                {
                    product = item;
                    break;
                }
            }

            if (product == null)
            {
                return null;
            }

            ProductDto productDto = new ProductDto();

            productDto.Sku = product.Sku;
            productDto.Name = product.Name;
            productDto.Price = product.Price;
            productDto.Category = product.Category;
            productDto.Stock = product.Stock;

            return productDto;
        }

        // Create a new product
        public ProductDto? Create(CreateProductDto productDto)
        {
            foreach (Product product in _data.Products)
            {
                if (product.Sku.Equals(
                    productDto.Sku,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
            }

            Product newProduct = new Product();

            newProduct.Sku = productDto.Sku;
            newProduct.Name = productDto.Name;
            newProduct.Price = productDto.Price;
            newProduct.Category = productDto.Category;
            newProduct.Stock = productDto.Stock;

            _data.Products.Add(newProduct);

            ProductDto result = new ProductDto();

            result.Sku = newProduct.Sku;
            result.Name = newProduct.Name;
            result.Price = newProduct.Price;
            result.Category = newProduct.Category;
            result.Stock = newProduct.Stock;

            return result;
        }

        // Update an existing product
        public ProductDto? Update(
            string sku,
            UpdateProductDto productDto)
        {
            Product? product = null;

            foreach (Product item in _data.Products)
            {
                if (item.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase))
                {
                    product = item;
                    break;
                }
            }

            if (product == null)
            {
                return null;
            }

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Category = productDto.Category;
            product.Stock = productDto.Stock;

            ProductDto result = new ProductDto();

            result.Sku = product.Sku;
            result.Name = product.Name;
            result.Price = product.Price;
            result.Category = product.Category;
            result.Stock = product.Stock;

            return result;
        }

        // Delete a product
        public bool Delete(string sku)
        {
            Product? product = null;

            foreach (Product item in _data.Products)
            {
                if (item.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase))
                {
                    product = item;
                    break;
                }
            }

            if (product == null)
            {
                return false;
            }

            _data.Products.Remove(product);

            return true;
        }
    }
}