using System.Text.Json.Serialization;
using Kaboom.Models.product;

namespace Kaboom.Models.StockModel
{
    public class Stocks
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Products  Products { get; set; }
        public int Quantity { get; set; } //Total Quantity avaliable
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
