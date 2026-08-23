using System.ComponentModel.DataAnnotations;

namespace POSSystem2.DTOs.Products
{
    public class UpdateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;
        
        [Required]
        public int Stock { get; set; }
    }
}
