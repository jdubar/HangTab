using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class DatabaseRepository(IDatabaseContext context) : IDatabaseRepository
{
    public async Task<bool> DropAllTables()
    {
        return await context.DropTableAsync<Person>()
            && await context.DropTableAsync<Week>()
            && await context.DropTableAsync<Bowler>();
    }

    public Task InitializeDatabase()
    {
        return Task.WhenAll(
            context.CreateTableIfNotExists<Person>(),
            context.CreateTableIfNotExists<Week>(),
            context.CreateTableIfNotExists<Bowler>()
        );
    }
}
