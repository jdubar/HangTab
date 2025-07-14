using System.Linq.Expressions;

namespace HangTab.Data;
public interface IDatabaseContext
{
    Task<bool> AddItemAsync<TTable>(TTable item) where TTable : class, new();
    Task<bool> DeleteItemByIdAsync<TTable>(object id) where TTable : class, new();
    Task<bool> ResetTableAsync<TTable>() where TTable : class, new();
    Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new();
    Task<IEnumerable<TTable>> GetFilteredAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new();
    Task<TTable> GetItemByIdAsync<TTable>(object id) where TTable : class, new();
    Task<bool> UpdateItemAsync<TTable>(TTable item) where TTable : class, new();

    Task CreateTableIfNotExists<TTable>() where TTable : class, new();

    Task<IEnumerable<TTable>> GetAllWithChildrenAsync<TTable>(Expression<Func<TTable, bool>>? predicate = null) where TTable : class, new();
    Task UpdateWithChildrenAsync<TTable>(TTable item) where TTable : class, new();
}
