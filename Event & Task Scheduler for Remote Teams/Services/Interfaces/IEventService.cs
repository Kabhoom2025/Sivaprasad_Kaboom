using Event___Task_Scheduler_for_Remote_Teams.Models;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventItem>> GetAllEventsAsync();
        Task<EventItem> GetEventByIdAsync(Guid id);
        Task<EventItem> CreateEventAsync(EventItem eventItem);
        Task<EventItem> UpdateEventAsync(EventItem eventItem);
        Task<bool> DeleteEventAsync(Guid id);
    }
}
