using Kaboom.Interfaces;
using MongoDB.Driver;

namespace Kaboom.Services.Repository
{
    public class MongoRepository<T> : IRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;
        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }
        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var idProperty = typeof(T).GetProperty("Id");
            var idvalue = idProperty.GetValue(entity, null);
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("Id", idvalue), entity);
        }

        IEnumerable<T> IRepository<T>.GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
