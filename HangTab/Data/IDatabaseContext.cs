using System.Linq.Expressions;

namespace HangTab.Data;
public interface IDatabaseContext
{
    Task<bool> AddItemAsync<TTable>(TTable item) where TTable : class, new();
    Task<bool> DeleteItemAsync<TTable>(TTable item) where TTable : class, new();
    Task<bool> DeleteItemByIdAsync<TTable>(object id) where TTable : class, new();
    Task<bool> DropTableAsync<TTable>() where TTable : class, new();
    Task<IReadOnlyCollection<TTable>> GetAllAsync<TTable>() where TTable : class, new();
    Task<IReadOnlyCollection<TTable>> GetFilteredAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new();
    Task<TTable> GetItemByIdAsync<TTable>(object id) where TTable : class, new();
    Task<bool> UpdateItemAsync<TTable>(TTable item) where TTable : class, new();
}
