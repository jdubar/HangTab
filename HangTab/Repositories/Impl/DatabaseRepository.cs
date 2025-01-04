using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class DatabaseRepository(IDatabaseContext context) : IDatabaseRepository
{
    public async Task<bool> DropAllTables()
        => await context.DropTableAsync<Bowler>(); // TODO: Add other tables
}
