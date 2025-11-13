using HangTab.Data;

using System.Linq.Expressions;

namespace HangTab.Repositories.Impl;
public partial class BaseRepository<T>(IDatabaseContext context) : IBaseRepository<T> where T : class, new()
{
    public Task<bool> AddAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return context.AddItemAsync(item);
    }

    public Task<bool> DeleteByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return context.DeleteItemByIdAsync<T>(id);
    }

    public Task<IEnumerable<T>> GetAllAsync() => context.GetAllWithChildrenAsync<T>();

    public Task<IEnumerable<T>> GetAllFilteredAsync(Expression<Func<T, bool>> predicate) => context.GetAllWithChildrenAsync(predicate);

    public Task<T> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return context.GetItemByIdAsync<T>(id);
    }

    public Task<IEnumerable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return context.GetFilteredAsync(predicate);
    }

    public Task<bool> UpdateAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return context.UpdateItemAsync(item);
    }
}
