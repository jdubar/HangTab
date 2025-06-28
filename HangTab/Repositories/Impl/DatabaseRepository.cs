using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class DatabaseRepository(IDatabaseContext context) : IDatabaseRepository
{
    public async Task<bool> DeleteAllData()
    {
        return await context.ResetTableAsync<Person>()
            && await context.ResetTableAsync<Week>()
            && await context.ResetTableAsync<Bowler>();
    }

    public async Task<bool> DeleteSeasonData()
    {
        return await context.ResetTableAsync<Week>()
            && await context.ResetTableAsync<Bowler>();
    }
}
