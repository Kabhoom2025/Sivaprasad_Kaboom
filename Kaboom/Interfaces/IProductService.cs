using Kaboom.Models.product;

namespace Kaboom.Interfaces
{
    public interface IProductService
    {
        List<Products> GetAllProducts();
        Products GetProductById(int id);
        Products AddProduct(Products product);
        Products UpdateProduct(int id, Products product);
        bool DeleteProduct(int id);
        bool UpdateStock(int productId, int newQuantity);// to update the current stock of the product
    }
}
