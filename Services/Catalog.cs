using System;
using System.Collections.Generic;
using System.Text;
using POSSystem.Models;
using POSSystem.Helpers;

namespace POSSystem.Services
{
    public class Catalog
    {
        public List<Product> Products= new List<Product>();
        public Dictionary<string, Product> ProductDictionary = new Dictionary<string, Product>();

        public void AddProduct(Product product)
        {
            if (ProductDictionary.ContainsKey(product.SKU))
            {
                var existing = ProductDictionary[product.SKU];
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.Stock = product.Stock;
                existing.Category = product.Category;

                int idx = Products.FindIndex(p => p.SKU == product.SKU);
                if (idx >= 0)
                {
                    Products[idx] = existing;
                }
                else
                {
                    Products.Add(existing);
                }
            }
            else
            {
                Products.Add(product);
                ProductDictionary[product.SKU] = product;
            }
        }

        public Product FindBySku(string sku)
        {
            if (ProductDictionary.ContainsKey(sku))
            {
                return ProductDictionary[sku];
            }

            return null;
        }

        public void ShowProducts()
        {
            Console.WriteLine();
            Console.WriteLine("===== PRODUCT LIST =====");

            foreach (Product product in Products)
            {
                product.Display();
            }
        }

    }
}
