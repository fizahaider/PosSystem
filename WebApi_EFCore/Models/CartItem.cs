namespace POSSystem2.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public string Sku { get; set; } = "";

        public string ProductName { get; set; } = "";

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Total
        {
            get
            {
                return UnitPrice * Quantity;
            }
        }

        public int OrderId { get; set; }

        public Order? Order { get; set; }

        public Product? Product { get; set; }
    }
}