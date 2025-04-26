using System;
using Event___Task_Scheduler_for_Remote_Teams.Data;
using Event___Task_Scheduler_for_Remote_Teams.Hubs;
using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public async Task<EventItem> CreateEventAsync(EventItem eventItem)
        {
            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();
            // Notify clients about the new event
            await _hubContext.Clients.All.SendAsync("ReceiveEvent", eventItem);
            return eventItem;
        }

        public async Task<bool> DeleteEventAsync(Guid id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null) return false;

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EventItem>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<EventItem> GetEventByIdAsync(Guid id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<EventItem> UpdateEventAsync(EventItem eventItem)
        {
            var existingEvent = await _context.Events.FindAsync(eventItem.Id);
            if (existingEvent == null) return null;

            existingEvent.Title = eventItem.Title;
            existingEvent.EventData = eventItem.EventData;

            await _context.SaveChangesAsync();
            return existingEvent; 
        }
    }
}
