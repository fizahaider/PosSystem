using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSSystem2.DTOs.Orders;
using POSSystem2.Services;

namespace POSSystem2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public ActionResult<OrderDto> CreateOrder(
            CreateOrderDto orderDto)
        {
            var order = _orderService.CreateOrder(orderDto);

            return CreatedAtAction(
                nameof(GetOrder),
                new { id = order.Id },
                order);
        }

        [HttpGet("{id:int}")]
        public ActionResult<OrderDto> GetOrder(int id)
        {
            var order = _orderService.GetOrder(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost("{id:int}/items")]
        public ActionResult<OrderDto> AddItem(
            int id,
            AddCartItemDto itemDto)
        {
            var order = _orderService.AddItem(id, itemDto);

            if (order == null)
            {
                return BadRequest(
                    "Unable to add item");
            }

            return Ok(order);
        }

        [HttpDelete("{id:int}/items/{sku}")]
        public IActionResult RemoveItem(
            int id,
            string sku)
        {
            var removed = _orderService.RemoveItem(id, sku);

            if (!removed)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id:int}/checkout")]
        public ActionResult<OrderDto> Checkout(int id)
        {
            var order = _orderService.Checkout(id);

            if (order == null)
            {
                return BadRequest(
                    "Unable to checkout.");
            }

            return Ok(order);
        }
    }
}
