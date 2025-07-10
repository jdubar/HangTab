using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
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

    public async Task InitializeDatabase()
    {
        await context.CreateTableIfNotExists<Person>();
        await context.CreateTableIfNotExists<Week>();
        await context.CreateTableIfNotExists<Bowler>();
    }
}
