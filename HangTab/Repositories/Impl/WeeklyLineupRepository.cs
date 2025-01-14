using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class WeeklyLineupRepository(IDatabaseContext context) : IWeeklyLineupRepository
{
    public Task<bool> AddWeeklyLineupBowler(WeeklyLineup weeklyLineup) => context.AddItemAsync(weeklyLineup);
    public Task<IEnumerable<WeeklyLineup>> GetWeeklyLineupsByWeekId(int weekId) => context.GetFilteredAsync<WeeklyLineup>(wl => wl.WeekId == weekId);
}
