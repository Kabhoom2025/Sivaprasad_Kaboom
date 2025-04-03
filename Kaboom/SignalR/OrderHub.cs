using Microsoft.AspNetCore.SignalR;

namespace Kaboom.SignalR
{
    public class OrderHub:Hub
    {
        public Task NotifyStockUpdate(int productId, int newStock)
        {
           return  Clients.All.SendAsync("RecieveStockUpdate",productId,newStock);
        }
        public Task NotifyOrderStatus(int orderId, int status)
        {
            return Clients.All.SendAsync("RecieveOrderStatus",orderId,status);
        }
    }
}
