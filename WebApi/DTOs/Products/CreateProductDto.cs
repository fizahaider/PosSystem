namespace POSSystem2.DTOs.Products
{
    public class CreateProductDto
    {
        public string Sku { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;

        public int Stock { get; set; }
    }
}
