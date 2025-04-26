using System.ComponentModel.DataAnnotations.Schema;

namespace Event___Task_Scheduler_for_Remote_Teams.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public Dictionary<string, object> ProjectData { get; set; } = new();
    }
}
