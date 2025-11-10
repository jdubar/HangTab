namespace HangTab.Repositories;
public interface IBaseRepository<T> where T : class, new()
{
    Task<bool> AddAsync(T item);
    Task<bool> DeleteByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetFilteredAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
    Task<bool> UpdateAsync(T item);
}
