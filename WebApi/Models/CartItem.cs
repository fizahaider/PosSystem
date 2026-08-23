namespace POSSystem2.Models
{
    public class CartItem
    {
        public string Sku { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Total => UnitPrice * Quantity;
    }
}
