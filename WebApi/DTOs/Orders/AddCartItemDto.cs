namespace POSSystem2.DTOs.Orders
{
    public class AddCartItemDto
    {
        public string Sku { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }
}
