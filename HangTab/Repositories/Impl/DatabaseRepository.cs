using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class DatabaseRepository(IDatabaseContext context) : IDatabaseRepository
{
    public async Task<bool> DropAllTables()
    {
        return await context.DropTableAsync<Bowler>()
            && await context.DropTableAsync<Week>()
            && await context.DropTableAsync<WeeklyLineup>();
    }
}
