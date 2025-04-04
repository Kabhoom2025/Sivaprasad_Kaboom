using System.Text.Json.Serialization;
using Kaboom.Models.product;

namespace Kaboom.Models.Order
{
    public class OrderItems
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [JsonIgnore]  // Prevent circular reference during serialization 
        public Orders Orders { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }  // Price at the time of order

    }
}
