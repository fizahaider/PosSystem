namespace POSSystem2.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public List<CartItemDto> Items { get; set; } = new();

        public decimal Total { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
