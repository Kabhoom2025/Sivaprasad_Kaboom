using System.Security.Claims;
using Kaboom.Interfaces;
using Kaboom.Models;
using Kaboom.Models.UltimateModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kaboom.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
       // [Authorize]
        [HttpPost("placeorder")]
        public IActionResult PlaceOrder([FromBody] List<OrderItem> orderItem)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(userId == null)
            {
                return NotFound(new { Message = "UserId not Found" });
            }
            var order = _orderService.PlaceOrder(userId, orderItem);
            return Ok(order);
        }
        //[Authorize]
        [HttpGet("user-orders")]
        public IActionResult GetUserOrder()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == null)
            {
                return BadRequest();
            }
            var orders = _orderService.GetOrderByUserId(userId);
            return Ok(orders);  
        }
      //  [Authorize(Roles ="Admin")]
        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            var order = _orderService.GetAllOrders();
            return Ok(order);
        }
       // [Authorize(Roles = "Admin")]
        [HttpGet("{orderId}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _orderService.GetOrderById(id);
            if(order==null)
                return NotFound(new {Message = "Order not Found"});
            return Ok(order);   
        }
    }
}
