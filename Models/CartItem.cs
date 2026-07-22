using System;
using System.Collections.Generic;
using System.Text;
namespace POSSystem.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal total => Product.Price * Quantity;
        public CartItem (Product product,  int quantity)
        { 
            Product= product;
            Quantity= quantity;
        }
    }
}
