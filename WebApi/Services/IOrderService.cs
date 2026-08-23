using POSSystem2.DTOs.Orders;

namespace POSSystem2.Services
{
    public interface IOrderService
    {
        OrderDto CreateOrder(CreateOrderDto orderDto);

        OrderDto? GetOrder(int id);

        OrderDto? AddItem(
            int orderId,
            AddCartItemDto itemDto);

        bool RemoveItem(
            int orderId,
            string sku);

        OrderDto? Checkout(int orderId);
    }
}