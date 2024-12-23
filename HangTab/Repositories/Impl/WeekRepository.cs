using HangTab.Data.Impl;

namespace HangTab.Repositories;
public class WeekRepository(DatabaseContext context) : IWeekRepository
{
    public async Task<Week> GetWeek(int weekNumber)
    {
        var weeks = await context.GetAllAsync<Week>();
        var week = weeks.FirstOrDefault(w => w.WeekNumber == weekNumber);
        return week ?? new Week();
    }
}
