using Kaboom.Models.product;

namespace Kaboom.Interfaces
{
    public interface IProductService
    {
        List<Products> GetAllProducts();
        Products GetProductById(int id);
        Products AddProduct(Products product);
        Products UpdateProduct(int id,Products product);
        bool DeleteProduct(int id);
    }
}
