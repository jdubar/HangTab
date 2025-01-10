using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class WeekRepository(IDatabaseContext context) : IWeekRepository
{
    public Task<Week> GetWeek(int id) => context.GetItemByIdAsync<Week>(id);
    public Task<IEnumerable<Week>> GetWeeks() => context.GetAllAsync<Week>();
    public async Task<Week> GetWeekByWeekNumber(int weekNumber)
    {
        var weeks = await context.GetFilteredAsync<Week>(w => w.WeekNumber == weekNumber);
        return weeks is null || !weeks.Any()
            ? new Week()
            : weeks.First();
    }
}
