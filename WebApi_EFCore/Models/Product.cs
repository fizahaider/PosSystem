namespace POSSystem2.Models
{
        public class Product
        {
            public string Sku { get; set; } = "";

            public string Name { get; set; } = "";

            public decimal Price { get; set; }

            public string Category { get; set; } = "";

            public int Stock { get; set; }

            public List<CartItem> CartItems { get; set; } = new();
        }
    }

