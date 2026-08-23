using System.ComponentModel.DataAnnotations;
namespace POSSystem2.DTOs.Orders
{
    public class AddCartItemDto
    {
        [Required]
        public string Sku { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }
    }
}
