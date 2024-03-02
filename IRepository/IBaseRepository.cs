using System.Linq.Expressions;

namespace Comment_Post.IRepository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<int> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
