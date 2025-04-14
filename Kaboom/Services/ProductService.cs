using Kaboom.Interfaces;
using Kaboom.Models;
using Kaboom.Models.product;
using Kaboom.Models.StockModel;
using MongoDB.Driver;

namespace Kaboom.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataBaseService _dataBaseService;
        private readonly IMongoDatabase _mongoDB;
        private readonly IAuthService _authService;
        public ProductService(ApplicationDbContext context, IDataBaseService dataBaseService, IMongoClient mongoClient, IAuthService authService)
        {
            _context = context;
            _dataBaseService = dataBaseService;
            _mongoDB = mongoClient.GetDatabase("MongoDbConnection"); ;
            _authService = authService;
        }
        public Products AddProduct(Products product)
        {
            var data = new Products()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                ProductImageUrl = product.ProductImageUrl,
                ProductStock = product.ProductStock
            };
            _context.Products.Add(data);
            _context.SaveChanges();
            return data;
        }

        public bool DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }

        public List<Products> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Products GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public Products UpdateProduct(int id, Products product)
        {
            var existingProduct = _context.Products.Find(id);
            if (existingProduct == null)
            {
                return null;
            }
            existingProduct.ProductName = product.ProductName ?? existingProduct.ProductName;
            existingProduct.ProductDescription = product.ProductDescription ?? existingProduct.ProductDescription;
            existingProduct.ProductPrice = product.ProductPrice != 0 ? product.ProductPrice : existingProduct.ProductPrice;
            existingProduct.ProductImageUrl = product.ProductImageUrl ?? existingProduct.ProductImageUrl;
            existingProduct.ProductStock = product.ProductStock != 0 ? product.ProductStock : existingProduct.ProductStock;

            _context.SaveChanges();
            return existingProduct;
        }

        public bool UpdateStock(int productId, int newQuantity)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return false;
            }
            product.ProductStock += newQuantity;
            _context.Update(product);
            var stockentry = new Stocks
            {
                ProductId = productId,
                Quantity = product.ProductStock,
                LastUpdated = DateTime.UtcNow
            };
            _context.Stocks.Update(stockentry);
            _context.SaveChanges();
            return true;
        }
    }
}
