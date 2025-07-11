using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
public class DatabaseRepository(IDatabaseContext context) : IDatabaseRepository
{
    public async Task<bool> DeleteAllData()
    {
        return await context.ResetTableAsync<Person>()
            && await context.ResetTableAsync<Bowler>()
            && await context.ResetTableAsync<Week>();
    }

    public async Task<bool> DeleteSeasonData()
    {
        return await context.ResetTableAsync<Bowler>()
            && await context.ResetTableAsync<Week>();
    }

    public async Task InitializeDatabase()
    {
        await context.CreateTableIfNotExists<Person>();
        await context.CreateTableIfNotExists<Bowler>();
        await context.CreateTableIfNotExists<Week>();
    }
}
