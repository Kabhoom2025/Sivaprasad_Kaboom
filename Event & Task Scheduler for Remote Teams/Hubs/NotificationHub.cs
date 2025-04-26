using Microsoft.AspNetCore.SignalR;

namespace Event___Task_Scheduler_for_Remote_Teams.Hubs
{
    public class NotificationHub:Hub
    {
        public async Task SendTaskUpdate(string taskId, string status)
        {
            await Clients.All.SendAsync("ReceiveTaskUpdate", taskId, status);
        }

        public async Task SendEventUpdate(string eventId, string status)
        {
            await Clients.All.SendAsync("ReceiveEventUpdate", eventId, status);
        }
    }
}
