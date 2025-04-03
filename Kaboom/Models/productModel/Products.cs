namespace Kaboom.Models.product
{
    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductStock { get; set; } // Available Stock
        public string ProductImageUrl { get; set; } // Product image
    }
}
