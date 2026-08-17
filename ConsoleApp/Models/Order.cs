using POSSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace POSSystem.Models
{
    public class Order
    {
        public string CustomerName { get; set; }
        private List<CartItem> items = new List<CartItem>();

        public List<CartItem> Items
        {
            get { return items; }
        }

        public Order(string customername) { 
            CustomerName = customername;
        }

        public void AddItem(Product product, int quantity)
        {
            var item = items.FirstOrDefault(x => x.Product.SKU== product.SKU);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                items.Add(new CartItem(product, quantity));
            }
        }

        public void RemoveOrder (string sku)
        {
            items.RemoveAll(x=> x.Product.SKU == sku);
        }

        public void RemoveQuantity(string sku, int quantity)
        {
            var item = items.FirstOrDefault(x => x.Product.SKU == sku);
            if (item == null) return;

            item.Quantity -= quantity;
            if (item.Quantity <= 0)
            {
                items.Remove(item);
            }
        }

        public decimal Total
        {
            get
            {
                decimal t = items.Sum(x=> x.total);
                decimal taxhelper = TaxHelper.CalculateTax(t);
                return t + taxhelper;
            }
        }

        public string GenerateReceipt()
        {
            StringBuilder sb = new();
            sb.AppendLine("Receipt");
            sb.AppendLine("Customer: " + CustomerName);

            foreach (CartItem item in items)
            {
                sb.AppendLine($"{item.Product.Name} x {item.Quantity} = {MoneyHelper.Format(item.total)}");
            }

            sb.AppendLine("Grand Total: " + MoneyHelper.Format(Total));

            return sb.ToString();
        }

    
    }
    }
