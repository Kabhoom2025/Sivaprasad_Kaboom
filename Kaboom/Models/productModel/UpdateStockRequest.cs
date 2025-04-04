namespace Kaboom.Models.productModel
{
    public class UpdateStockRequest
    {
        public int ProductId { get; set; }
        public int NewQuantity { get; set; }
    }
}
