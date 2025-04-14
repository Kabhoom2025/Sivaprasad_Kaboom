namespace Kaboom.Models.Order
{
    public class Orders
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? AdminId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
        public decimal TotalAmount { get; set; }


    }
}
