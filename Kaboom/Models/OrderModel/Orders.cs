

using Kaboom.Models.Admin;
using Kaboom.Models.Users;

namespace Kaboom.Models.Order
{
    public class Orders
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public Users.Users Users { get; set; }
        public int? AdminId { get; set; }
        public Admins Admins { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
        public decimal TotalAmount { get; set; }


    }
}
