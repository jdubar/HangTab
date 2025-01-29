using HangTab.Shared;

using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

using System.Linq.Expressions;

namespace HangTab.Data.Impl;
public class DatabaseContext : IDatabaseContext, IAsyncDisposable
{
    private static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, Constants.DatabaseName);

    private SQLiteAsyncConnection? _connection;
    private SQLiteAsyncConnection Database =>
        _connection ??= new SQLiteAsyncConnection(DatabasePath,
            SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);

    public async Task<bool> AddItemAsync<TTable>(TTable item) where TTable : class, new() =>
        await Execute<TTable, bool>(async () => await Database.InsertAsync(item) > 0);

    public async Task<bool> DeleteItemAsync<TTable>(TTable item) where TTable : class, new() =>
        await Execute<TTable, bool>(async () => await Database.DeleteAsync(item) > 0);

    public async Task<bool> DeleteItemByIdAsync<TTable>(object id) where TTable : class, new() =>
        await Execute<TTable, bool>(async () => await Database.DeleteAsync<TTable>(id) > 0);

    public async Task<bool> DropTableAsync<TTable>() where TTable : class, new() =>
        await Execute<TTable, bool>(async () => await Database.DropTableAsync<TTable>() > 0);

    public async Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new()
    {
        var table = await GetTableAsync<TTable>();
        return await table.ToListAsync();
    }

    public async Task<IEnumerable<TTable>> GetFilteredAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new()
    {
        var table = await GetTableAsync<TTable>();
        return await table.Where(predicate).ToListAsync();
    }

    public async Task<TTable> GetItemByIdAsync<TTable>(object id) where TTable : class, new() =>
        await Execute<TTable, TTable>(async () => await Database.GetAsync<TTable>(id));

    public async Task<bool> UpdateItemAsync<TTable>(TTable item) where TTable : class, new() =>
        await Execute<TTable, bool>(async () => await Database.UpdateAsync(item) > 0);

    public async Task<TTable> GetWithChildrenAsync<TTable>(object id) where TTable : class, new() =>
        await Execute<TTable, TTable>(async () => await Database.GetWithChildrenAsync<TTable>(id, recursive: true));

    public async Task<IEnumerable<TTable>> GetAllWithChildrenAsync<TTable>(Expression<Func<TTable, bool>>? predicate = null) where TTable : class, new()
    {
        return await Execute<TTable, IEnumerable<TTable>>(async () =>
        {
            return await Database.GetAllWithChildrenAsync(predicate, recursive: true);
        });
    }

    public async Task InsertWithChildrenAsync<TTable>(TTable item) where TTable : class, new()
    {
        await Execute<TTable, Task>(() =>
        {
            return Task.FromResult(Database.InsertWithChildrenAsync(item, recursive: true));
        });
    }

    public async Task UpdateWithChildrenAsync<TTable>(TTable item) where TTable : class, new()
    {
        await Execute<TTable, Task>(() =>
        {
            return Task.FromResult(Database.UpdateWithChildrenAsync(item));
        });
    }

    public async Task CreateTableIfNotExists<TTable>() where TTable : class, new() =>
        await Database.CreateTableAsync<TTable>();

    private async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable : class, new()
    {
        await CreateTableIfNotExists<TTable>();
        return Database.Table<TTable>();
    }

    private async Task<TResult> Execute<TTable, TResult>(Func<Task<TResult>> action) where TTable : class, new()
    {
        await CreateTableIfNotExists<TTable>();
        return await action();
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.CloseAsync();
        }

        GC.SuppressFinalize(this);
    }
}
