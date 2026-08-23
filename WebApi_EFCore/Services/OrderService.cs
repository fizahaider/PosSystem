using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POSSystem2.Data;
using POSSystem2.DTOs.Orders;
using POSSystem2.Models;

namespace POSSystem2.Services
{
    public class OrderService : IOrderService
    {
        private readonly PosDbContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(PosDbContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Create a new order
        public OrderDto CreateOrder(CreateOrderDto orderDto)
        {
            Order newOrder = new Order
            {
                CustomerName = orderDto.CustomerName,
                Status = "Open"
            };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            return ConvertToDto(newOrder);
        }

        // Get an order by ID
        public OrderDto? GetOrder(int id)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return null;
            }

            return ConvertToDto(order);
        }

        // Add a product to an order
        public OrderDto? AddItem(
            int orderId,
            AddCartItemDto itemDto)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return null;
            }

            // Order must still be open
            if (order.Status != "Open")
            {
                return null;
            }

            // Quantity must be greater than zero
            if (itemDto.Quantity <= 0)
            {
                return null;
            }

            // Find product
            Product? product = _context.Products
                .FirstOrDefault(p => p.Sku == itemDto.Sku);

            if (product == null)
            {
                return null;
            }

            // Check if this product is already in the orders
            CartItem? existingItem = _context.CartItems
                .FirstOrDefault(c => c.OrderId == orderId && c.Sku == product.Sku);

            int requestedQuantity = itemDto.Quantity;

            if (existingItem != null)
            {
                requestedQuantity =
                    existingItem.Quantity + itemDto.Quantity;
            }

            // Check stock
            if (requestedQuantity > product.Stock)
            {
                return null;
            }

            // If product already exists in cart, update quantity; otherwise insert
            if (existingItem != null)
            {
                existingItem.Quantity += itemDto.Quantity;
                _context.CartItems.Update(existingItem);
                _context.SaveChanges();
            }
            else
            {
                CartItem newItem = new CartItem
                {
                    Sku = product.Sku,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = itemDto.Quantity,
                    OrderId = order.Id
                };

                _context.CartItems.Add(newItem);        
                _context.SaveChanges();
            }

            // Reload order with current items from DB to avoid in-memory duplicates
            var updatedOrder = _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == orderId);

            return updatedOrder == null ? null : ConvertToDto(updatedOrder);
        }

        // Remove an item from an order
        public bool RemoveItem(int orderId, string sku)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return false;
            }

            // Cannot change completed order
            if (order.Status != "Open")
            {
                return false;
            }

            var cartItem = order.Items.FirstOrDefault(i => i.Sku == sku);

            if (cartItem == null)
            {
                return false;
            }

            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            return true;
        }

        // Checkout an order
        public OrderDto? Checkout(int orderId)
        {
            // Load order with items
            var order = _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                _logger.LogWarning("Checkout failed: order {OrderId} not found", orderId);
                return null;
            }

            // Order must be open
            if (order.Status != "Open")
            {
                _logger.LogWarning("Checkout failed: order {OrderId} status is {Status}", orderId, order.Status);
                return null;
            }

            // Order must contain at least one item
            if (order.Items == null || order.Items.Count == 0)
            {
                _logger.LogWarning("Checkout failed: order {OrderId} has no items", orderId);
                return null;
            }

            // Check stock for every item first
            foreach (CartItem cartItem in order.Items)
            {
                var product = _context.Products.FirstOrDefault(p => p.Sku == cartItem.Sku);

                if (product == null)
                {
                    _logger.LogWarning("Checkout failed: product {Sku} not found for order {OrderId}", cartItem.Sku, orderId);
                    return null;
                }

                if (product.Stock < cartItem.Quantity)
                {
                    _logger.LogWarning("Checkout failed: insufficient stock for product {Sku} (requested {Requested}, available {Stock}) for order {OrderId}", cartItem.Sku, cartItem.Quantity, product.Stock, orderId);
                    return null;
                }
            }

            // Decrement stock
            foreach (CartItem cartItem in order.Items)
            {
                var product = _context.Products.First(p => p.Sku == cartItem.Sku);
                product.Stock -= cartItem.Quantity;
                _context.Products.Update(product);
            }

            // Complete order
            order.Status = "Completed";
            _context.Orders.Update(order);

            _context.SaveChanges();

            _logger.LogInformation("Order {OrderId} checked out successfully", orderId);

            return ConvertToDto(order);
        }

        // Convert Order model to Order DTO
        private OrderDto ConvertToDto(Order order)
        {
            OrderDto orderDto = new OrderDto();

            orderDto.Id = order.Id;
            orderDto.CustomerName = order.CustomerName;
            orderDto.Status = order.Status;
            orderDto.Total = order.CalculateTotal();

            foreach (CartItem item in order.Items)
            {
                CartItemDto cartItemDto = new CartItemDto();

                cartItemDto.Sku = item.Sku;
                cartItemDto.ProductName = item.ProductName;
                cartItemDto.UnitPrice = item.UnitPrice;
                cartItemDto.Quantity = item.Quantity;
                cartItemDto.Total = item.Total;

                orderDto.Items.Add(cartItemDto);
            }

            return orderDto;
        }
    }
}