namespace Kaboom.Models.UltimateModel
{
    public class UltimateData
    {
        //User Info
        public Users.Users Users { get; set; }


        //Procuct Info
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Desccription { get; set; }
        public string? ProductPrice { get; set; }
        public string? ProductStock { get; set; }
        public string ProductImageUrl { get; set; }

        // Order Info
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotalAmount { get; set; }

        // Order Items
        public List<OrderItem> OrderItemsdata { get; set; } = new List<OrderItem>();

        // Stock Quantity
        public string? StockQuantity { get; set; }

    }
    //public class OrderItemData
    //{
    //    public int ProductId { get; set; }
    //    public string? ProductName { get; set; }
    //    public int Quantity { get; set; }
    //    public decimal Price { get; set; }
    //    public string? ProductImageUrl { get; set; }
    //}
}
