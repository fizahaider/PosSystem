using System;
using System.Collections.Generic;
using System.Text;
using POSSystem.Helpers;

namespace POSSystem.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }

        public Product (string name, string sKU, decimal price, int stock, string category)
        {
            Name = name;
            SKU = sKU;
            Price = price;
            Stock = stock;
            Category = category;
        }

        public void Display()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"SKU: {SKU}");
            Console.WriteLine($"Price: {MoneyHelper.Format(Price)}");
            Console.WriteLine($"Category: {Category}");     
            Console.WriteLine($"Stock: {Stock}");
        }

        public override string ToString()
        {
            return $"Name: {Name} - SKU: {SKU} - Price: {MoneyHelper.Format(Price)} - Category: {Category} - Stock: {Stock}";
        }
    }
}
