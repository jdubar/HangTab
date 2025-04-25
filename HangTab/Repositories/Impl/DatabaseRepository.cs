using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class DatabaseRepository(IDatabaseContext context) : IDatabaseRepository
{
    public async Task<bool> DeleteAllTableData()
    {
        return await context.ResetTableAsync<Person>()
            && await context.ResetTableAsync<Week>()
            && await context.ResetTableAsync<Bowler>();
    }
}
