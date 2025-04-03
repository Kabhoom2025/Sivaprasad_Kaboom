using Kaboom.Interfaces;
using Kaboom.Models;
using Kaboom.Models.Order;
using Kaboom.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;

namespace Kaboom.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<OrderHub> _orderHub;

        public OrderService(ApplicationDbContext context, IHubContext<OrderHub> orderHub)
        {
            _context = context;
            _orderHub = orderHub;
        }
        public List<Orders> GetAllOrders()
        {
           var data = _context.Orders.Include(o=>o.OrderItems).ToList();
            return data;
        }

        public Orders GetOrderById(int orderId)
        {
            var data = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(or => or.Id == orderId);
            return data;
        }

        public List<Orders> GetOrderByUserId(int userId)
        {
            var data = _context.Orders.Where(o=>o.UserId == userId).ToList();  
            return data;
        }

        public Orders PlaceOrder(int userId, List<OrderItem> orderitems)
        {
            var user = _context.Users?.Find(userId);
            var admin = _context.Admins?.Find(userId);
            var auth = _context.AuthUser.Find(userId);
            if(user == null && admin == null && auth == null)
            {
                throw new Exception("User not Found");
            }
            var orders = new Orders
            {
                UserId = user?.Id,
                AdminId = admin?.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0,
                OrderItems = new List<OrderItems>()
            };
            foreach(var item in orderitems)
            {
                var product = _context.Products.Find(item.ProductId);
                if(product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not Found");
                }
                if(product.ProductStock < item.Quantity)
                {
                    throw new Exception($"Not Enough stock for {product.ProductName}");
                }
                decimal price = product.ProductPrice;
                //Reduce Stock
                product.ProductStock -= item.Quantity;
                orders.OrderItems.Add(new OrderItems
                {
                    ProductId = item.ProductId,
                    Price = price ,
                    Quantity = item.Quantity,
                });
                //send realtime stock updates
                _orderHub.Clients.All.SendAsync("ReceiveStockUpdate", product.Id, product.ProductStock);
            }
            _context.Orders.Add(orders);
            _context.SaveChanges();
            //notify adminfor new orders
            _orderHub.Clients.All.SendAsync("ReceiveNewOrder", orders.Id, user.UserName ?? admin.UserName);
            return orders;

            //new example for push
        }
    }
}
