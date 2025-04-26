namespace Event___Task_Scheduler_for_Remote_Teams.Models
{
    public class UserProfileRequest
    {
        public Dictionary<string, object> UserData { get; set; } = new();
    }
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public Guid AuthUserId { get; set; }
        public Dictionary<string, object> UserData { get; set; } = new();
    }
}
