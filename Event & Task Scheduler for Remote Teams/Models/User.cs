namespace Event___Task_Scheduler_for_Remote_Teams.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid AuthUserId { get; set; }
        public string Email { get; set; }
        public Dictionary<string, object> UserData { get; set; } = new();
        public List<Role> Roles { get; set; } = new();
        public AuthUser? AuthUser { get; set; }

    }
}
