
namespace POSSystem2.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = "";

        public string Status { get; set; } = "Open";

        public List<CartItem> Items { get; set; } = new();

        public decimal CalculateTotal()
        {
            decimal total = 0;

            foreach (CartItem item in Items)
            {
                total += item.Total;
            }

            return total;
        }
    }
}