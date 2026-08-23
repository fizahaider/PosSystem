namespace POSSystem2.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public List<CartItem> Items { get; set; } = new();

        public string Status { get; set; } = "Open";

        public decimal CalculateTotal()
        {
            return Items.Sum(item => item.Total);
        }
    }
}
