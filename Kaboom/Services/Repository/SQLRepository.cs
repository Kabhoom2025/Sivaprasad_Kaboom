using Kaboom.Interfaces;
using Kaboom.Models;
using Microsoft.EntityFrameworkCore;

namespace Kaboom.Services.Repository
{
    public class SQLRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public SQLRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
           await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entitty = await _dbSet.FindAsync(id);
            if (entitty != null)
            {
                _dbSet.Remove(entitty);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task<IEnumerable<T>> GetAllAsync()
        //{
        //    return await _dbSet.ToListAsync();
        //}

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        IEnumerable<T> IRepository<T>.GetAllAsync()
        {
            return (IEnumerable<T>)_dbSet.ToListAsync();
        }
    }
}
