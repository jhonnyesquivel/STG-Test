using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STG.Core.Entities;
using STG.Core.Services;
using System.Security.Claims;

namespace STG.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (order.Items.Count > 50)
            {
                return BadRequest("The number of animals cannot be greater than 50.");
            }

            foreach (var animal in order.Items)
            {
                if (await _orderService.IsAnimalAlreadyInOrder(order.OrderId, animal.AnimalId))
                {
                    return BadRequest($"The animal with ID {animal.AnimalId} is already in the order.");
                }
            }

            var placedOrder = await _orderService.PlaceOrder(order);
            return Ok(placedOrder);
        }
    }
}