namespace Event___Task_Scheduler_for_Remote_Teams.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> TeamData { get; set; } = new();
       
    }
}
