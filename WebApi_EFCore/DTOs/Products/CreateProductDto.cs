using System.ComponentModel.DataAnnotations;

namespace POSSystem2.DTOs.Products
{
    public class CreateProductDto
    {
        [Required]
        public string Sku { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;
        [Required]
        public int Stock { get; set; }
    }
}
