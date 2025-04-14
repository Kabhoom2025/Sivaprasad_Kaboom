namespace Kaboom.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        //IEnumerable<Admins> GetAll();
    }
}
