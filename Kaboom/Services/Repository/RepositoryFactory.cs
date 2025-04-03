using Kaboom.Interfaces;
using Kaboom.Models;
using MongoDB.Driver;
using MongoDB.Driver.Authentication;

namespace Kaboom.Services.Repository
{
    public class RepositoryFactory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMongoDatabase _mongoData;
        private readonly IDataBaseService _dataBaseService;
        public RepositoryFactory(ApplicationDbContext context, IMongoClient mongoClient, IDataBaseService dataBaseService)
        {
            _context = context;
            _mongoData = mongoClient.GetDatabase("OnlineShopDB"); // Replace with your DB name
            _dataBaseService = dataBaseService;
        }

        public IRepository<T> CreateRepository<T>(string collectionName="") where T : class 
        {
            string provider = _dataBaseService.GetCurrentDatabaseProvider();
            return provider switch
            {
                "SqlServerProvider" => new SQLRepository<T>(_context),
                "MongoDbProvider" => new MongoRepository<T>(_mongoData, collectionName),
                _ => throw new InvalidOperationException($"{provider}"+"Invalid database provider selected")
            };
        }
    }
}
