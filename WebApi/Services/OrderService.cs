using POSSystem2.Data;
using POSSystem2.DTOs.Orders;
using POSSystem2.Models;

namespace POSSystem2.Services
{
    public class OrderService : IOrderService
    {
        private readonly InMemoryData _data;

        public OrderService(InMemoryData data)
        {
            _data = data;
        }

        // Create a new order
        public OrderDto CreateOrder(CreateOrderDto orderDto)
        {
            int newId = 1;

            if (_data.Orders.Count > 0)
            {
                int highestId = 0;

                foreach (Order order in _data.Orders)
                {
                    if (order.Id > highestId)
                    {
                        highestId = order.Id;
                    }
                }

                newId = highestId + 1;
            }

            Order newOrder = new Order();

            newOrder.Id = newId;
            newOrder.CustomerName = orderDto.CustomerName;
            newOrder.Status = "Open";

            _data.Orders.Add(newOrder);

            return ConvertToDto(newOrder);
        }

        // Get an order by ID
        public OrderDto? GetOrder(int id)
        {
            Order? order = null;

            foreach (Order item in _data.Orders)
            {
                if (item.Id == id)
                {
                    order = item;
                    break;
                }
            }

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
            Order? order = null;

            // Find order
            foreach (Order item in _data.Orders)
            {
                if (item.Id == orderId)
                {
                    order = item;
                    break;
                }
            }

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
            Product? product = null;

            foreach (Product item in _data.Products)
            {
                if (item.Sku.Equals(
                    itemDto.Sku,
                    StringComparison.OrdinalIgnoreCase))
                {
                    product = item;
                    break;
                }
            }

            if (product == null)
            {
                return null;
            }

            // Check if this product is already in the order
            CartItem? existingItem = null;

            foreach (CartItem item in order.Items)
            {
                if (item.Sku.Equals(
                    product.Sku,
                    StringComparison.OrdinalIgnoreCase))
                {
                    existingItem = item;
                    break;
                }
            }

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

            // If product already exists in cart
            if (existingItem != null)
            {
                existingItem.Quantity =
                    existingItem.Quantity + itemDto.Quantity;
            }
            else
            {
                CartItem newItem = new CartItem();

                newItem.Sku = product.Sku;
                newItem.ProductName = product.Name;
                newItem.UnitPrice = product.Price;
                newItem.Quantity = itemDto.Quantity;

                order.Items.Add(newItem);
            }

            return ConvertToDto(order);
        }

        // Remove an item from an order
        public bool RemoveItem(int orderId, string sku)
        {
            Order? order = null;

            // Find order
            foreach (Order item in _data.Orders)
            {
                if (item.Id == orderId)
                {
                    order = item;
                    break;
                }
            }

            if (order == null)
            {
                return false;
            }

            // Cannot change completed order
            if (order.Status != "Open")
            {
                return false;
            }

            CartItem? cartItem = null;

            // Find item in cart
            foreach (CartItem item in order.Items)
            {
                if (item.Sku.Equals(
                    sku,
                    StringComparison.OrdinalIgnoreCase))
                {
                    cartItem = item;
                    break;
                }
            }

            if (cartItem == null)
            {
                return false;
            }

            order.Items.Remove(cartItem);

            return true;
        }

        // Checkout an order
        public OrderDto? Checkout(int orderId)
        {
            Order? order = null;

            // Find order
            foreach (Order item in _data.Orders)
            {
                if (item.Id == orderId)
                {
                    order = item;
                    break;
                }
            }

            if (order == null)
            {
                return null;
            }

            // Order must be open
            if (order.Status != "Open")
            {
                return null;
            }

            // Order must contain at least one item
            if (order.Items.Count == 0)
            {
                return null;
            }

            // Check stock for every item first
            foreach (CartItem cartItem in order.Items)
            {
                Product? product = null;

                foreach (Product item in _data.Products)
                {
                    if (item.Sku.Equals(
                        cartItem.Sku,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        product = item;
                        break;
                    }
                }

                if (product == null)
                {
                    return null;
                }

                if (product.Stock < cartItem.Quantity)
                {
                    return null;
                }
            }

            // Reduce stock
            foreach (CartItem cartItem in order.Items)
            {
                Product? product = null;

                foreach (Product item in _data.Products)
                {
                    if (item.Sku.Equals(
                        cartItem.Sku,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        product = item;
                        break;
                    }
                }

                if (product != null)
                {
                    product.Stock =
                        product.Stock - cartItem.Quantity;
                }
            }

            // Complete order
            order.Status = "Completed";

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