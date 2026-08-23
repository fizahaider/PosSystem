using POSSystem2.DTOs.Products;

namespace POSSystem2.Services
{
    public interface IProductService
    {
        List<ProductDto> GetAll();

        ProductDto? GetBySku(string sku);

        ProductDto? Create(CreateProductDto productDto);

        ProductDto? Update(string sku, UpdateProductDto productDto);

        bool Delete(string sku);
    }
}