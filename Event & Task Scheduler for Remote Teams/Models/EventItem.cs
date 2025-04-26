namespace Event___Task_Scheduler_for_Remote_Teams.Models
{
    public class EventItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Dictionary<string, object> EventData { get; set; } = new();
    }
}
