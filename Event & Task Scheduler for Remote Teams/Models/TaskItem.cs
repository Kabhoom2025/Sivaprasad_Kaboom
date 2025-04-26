namespace Event___Task_Scheduler_for_Remote_Teams.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Dictionary<string, object> TaskData { get; set; } = new();
    }
}
