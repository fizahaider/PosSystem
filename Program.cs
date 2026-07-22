using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using POSSystem.Helpers;
using POSSystem.Models;
using POSSystem.Services;

Catalog catalog = new Catalog();
PaymentService paymentService = new PaymentService();
await paymentService.LoadCatalogAsync();
var loaded = CatalogService.LoadCatalog();
if (loaded != null && loaded.Count > 0)
{
    foreach (Product p in loaded)
    {
        catalog.AddProduct(p);
    }
    Console.WriteLine($"Loaded {loaded.Count} products");
}
else
{
    catalog.AddProduct(new Product("Milk", "1", 250, 20, "Food"));
    catalog.AddProduct(new Product("Bread", "2", 120, 15, "Food"));
    catalog.AddProduct(new Product("Soap", "3", 180, 12, "Cleaning"));
    catalog.AddProduct(new Product("Juice", "4", 300, 10, "Drink"));
}
Repository<Product> repository = new Repository<Product>();
foreach (Product product in catalog.Products)
{
    repository.Add(product);
}
Order order = new Order("Walk-In Customer");
Stack<CartItem> undoStack = new Stack<CartItem>();
Queue<Order> orderQueue = new Queue<Order>();

Task backgroundProcessor = Task.Run(async () =>
{
    while (true)
    {
        Order toProcess = null;
        if (orderQueue.Count > 0)
            toProcess = orderQueue.Dequeue();

        if (toProcess != null)
        {
            Console.WriteLine("[Background] Processing archived order...");
            await Task.Delay(500);
            Console.WriteLine("[Background] Archived order processed: " + toProcess.CustomerName);
        }
        else
        {
            await Task.Delay(200);
        }
    }
});

string[] categories =
{
    "Food",
    "Drink",
    "Cleaning"
};

Console.WriteLine("Available Categories");

foreach (string category in categories)
{
    Console.WriteLine(category);
}

bool running = true;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("========== POS MENU ==========");
    Console.WriteLine("1. Show Products");
    Console.WriteLine("2. Add Product");
    Console.WriteLine("3. Start New Sale");
    Console.WriteLine("4. Add Item To Cart");
    Console.WriteLine("5. Remove Item");
    Console.WriteLine("6. Undo Last Add");
    Console.WriteLine("7. Checkout");
    Console.WriteLine("8. Save Catalog");
    Console.WriteLine("9. Exit");

    Console.Write("Choice : ");
    string choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            catalog.ShowProducts();
            break;

        case "2":
            Console.Write("SKU : ");
            string sku = Console.ReadLine();
            Console.Write("Name : ");
            string name = Console.ReadLine();
            Console.Write("Price : ");
            decimal price;
            while (!InputHelper.TryParsePrice(Console.ReadLine(), out price))
            {
                Console.Write("Invalid Price. Enter Again : ");
            }
            Console.Write("Category : ");
            string category = Console.ReadLine();
            Console.Write("Stock : ");
            int stock;
            while (!int.TryParse(Console.ReadLine(), out stock) || stock < 0)
            {
                Console.Write("Invalid stock. Enter a non-negative integer: ");
            }

            Product newProduct = new Product(
                name, sku,
                price,  stock,
                category
                );
            catalog.AddProduct(newProduct);
            repository.Add(newProduct);
            Console.WriteLine("Product Added.");
            break;

        case "3":
            Console.Write("Customer Name (leave blank for Walk-In): ");
            string customerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(customerName))
            {
                customerName = "Walk-In Customer";
            }
            order = new Order(customerName);
            undoStack.Clear();
            Console.WriteLine($"Started new sale for: {customerName}");
            break;

        case "4":
            Console.Write("Enter SKU : ");
            string searchSku = Console.ReadLine();
            Product product = catalog.FindBySku(searchSku);

            if (product == null)
            {
                Console.WriteLine("Product Not Found.");
                break;
            }
            Console.Write("Quantity : ");
            int quantity;
            while (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                Console.Write("Invalid quantity. Enter a positive integer: ");
            }
            if (quantity > product.Stock)
            {
                Console.WriteLine($"Insufficient stock. Available: {product.Stock}");
                break;
            }

            order.AddItem(product, quantity);
            int currentStock = product.Stock;
            StockHelper.ReduceStock(ref currentStock, quantity);
            product.Stock = currentStock;
            undoStack.Push(new CartItem(product, quantity));
            Console.WriteLine("Added To Cart.");
            break;

        case "5":
            Console.Write("Enter SKU : ");
            string removeSku = Console.ReadLine();
            var cartItem = order.Items.FirstOrDefault(i => i.Product.SKU == removeSku);
            if (cartItem != null)
            {
                var prod = catalog.FindBySku(removeSku);
                if (prod != null)
                {
                    prod.Stock += cartItem.Quantity;
                }
            }
            order.RemoveOrder(removeSku);
            Console.WriteLine("Item Removed.");
            break;

        case "6":
            if (undoStack.Count > 0)
            {
                CartItem lastItem = undoStack.Pop();
                order.RemoveQuantity(lastItem.Product.SKU, lastItem.Quantity);
                var prodUndo = catalog.FindBySku(lastItem.Product.SKU);
                if (prodUndo != null)
                prodUndo.Stock += lastItem.Quantity;
                Console.WriteLine("Undo Successful (quantity reverted).");
            }
            else
            {
                Console.WriteLine("Nothing To Undo.");
            }
            break;

        case "7":
            Order currentOrder = order;
            foreach (CartItem item in currentOrder.Items)
            {
                int s = item.Product.Stock;
                StockHelper.ReduceStock(ref s, item.Quantity);
                item.Product.Stock = s;
            }            
            await paymentService.ProcessAsync();
            string receipt = currentOrder.GenerateReceipt();
            Console.WriteLine();
            Console.WriteLine(receipt);
            
            // Directory.SetCurrentDirectory removed per user preference
            using (StreamWriter writer = new StreamWriter("receipt.txt"))
{
    writer.WriteLine(receipt);
}
            Console.WriteLine($"Receipt saved to {Path.GetFullPath("receipt.txt")}");
            Console.WriteLine("Receipt Saved.");
            CatalogService.SaveCatalog(catalog.Products);
            orderQueue.Enqueue(currentOrder);
            order = new Order("Walk-In Customer");
            break;
        
        case "8": 
            CatalogService.SaveCatalog(catalog.Products);
            break;
        
        case "9":
            running = false;
            break;

        default:
            Console.WriteLine("Invalid Choice.");
            break;
    }
}
Console.WriteLine("Thank You");