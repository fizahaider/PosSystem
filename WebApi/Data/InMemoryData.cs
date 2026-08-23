using POSSystem2.Models;

namespace POSSystem2.Data
{
    public class InMemoryData
    {
        public List<Product> Products { get; } = new()
    {
        new Product
        {
            Sku = "P001",
            Name = "Milk",
            Price = 250,
            Category = "Dairy",
            Stock = 20
        },

        new Product
        {
            Sku = "P002",
            Name = "Bread",
            Price = 180,
            Category = "Bakery",
            Stock = 30
        },

        new Product
        {
            Sku = "P003",
            Name = "Eggs",
            Price = 320,
            Category = "Dairy",
            Stock = 50
        }
    };

        public List<Order> Orders { get; } = new();
    }
}
