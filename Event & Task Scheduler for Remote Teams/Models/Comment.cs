namespace Event___Task_Scheduler_for_Remote_Teams.Models
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid LinkedEntityId { get; set; } // TaskId or EventId
        public string EntityType { get; set; } // "Task" or "Event"
        public Dictionary<string, object> CommentData { get; set; } = new();
    }
}
