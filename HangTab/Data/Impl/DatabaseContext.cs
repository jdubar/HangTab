using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

using System.Linq.Expressions;

namespace HangTab.Data.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test database context. There's no logic to test.")]
public class DatabaseContext : IDatabaseContext, IAsyncDisposable
{
    private SQLiteAsyncConnection? _connection;
    private static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, Constants.Database.FileName);
    private SQLiteAsyncConnection Database => _connection ??= new SQLiteAsyncConnection(DatabasePath, Constants.Database.OpenFlags);

    public async Task<bool> AddItemAsync<TTable>(TTable item) where TTable : class, new() => await Execute<TTable, bool>(async () => await Database.InsertAsync(item) > 0);

    public async Task<bool> DeleteItemByIdAsync<TTable>(object id) where TTable : class, new() => await Execute<TTable, bool>(async () => await Database.DeleteAsync<TTable>(id) > 0);

    public async Task<bool> ResetTableAsync<TTable>() where TTable : class, new()
    {
        var result = await DropTableAsync<TTable>();
        await CreateTableIfNotExists<TTable>();
        return result;
    }

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

    public async Task<TTable> GetItemByIdAsync<TTable>(object id) where TTable : class, new() => await Execute<TTable, TTable>(async () => await Database.GetAsync<TTable>(id));

    public async Task<bool> UpdateItemAsync<TTable>(TTable item) where TTable : class, new() => await Execute<TTable, bool>(async () => await Database.UpdateAsync(item) > 0);

    public async Task<IEnumerable<TTable>> GetAllWithChildrenAsync<TTable>(Expression<Func<TTable, bool>>? predicate = null) where TTable : class, new()
    {
        return await Execute<TTable, IEnumerable<TTable>>(async () =>
        {
            return await Database.GetAllWithChildrenAsync(predicate, recursive: true);
        });
    }

    public async Task UpdateWithChildrenAsync<TTable>(TTable item) where TTable : class, new()
    {
        await Execute<TTable, Task>(() =>
        {
            return Task.FromResult(Database.UpdateWithChildrenAsync(item));
        });
    }

    public async Task CreateTableIfNotExists<TTable>() where TTable : class, new() => await Database.CreateTableAsync<TTable>();

    private async Task<bool> DropTableAsync<TTable>() where TTable : class, new() => await Database.DropTableAsync<TTable>() > 0;

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
