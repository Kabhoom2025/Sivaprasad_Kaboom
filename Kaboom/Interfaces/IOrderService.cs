using Kaboom.Models;
using Kaboom.Models.Order;

namespace Kaboom.Interfaces
{
    public interface IOrderService
    {
        //void CreateOrder(Orders orders);
        List<Orders> GetOrderByUserId(int userId);
        List<Orders> GetAllOrders();
        Orders GetOrderById(int orderId);
        Orders PlaceOrder(int userId, List<OrderItem> orderitems);
    }
}
