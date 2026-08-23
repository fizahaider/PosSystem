namespace POSSystem2.DTOs.Orders
{
    public class CartItemDto
    {
        public string Sku { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Total { get; set; }
    }
}
