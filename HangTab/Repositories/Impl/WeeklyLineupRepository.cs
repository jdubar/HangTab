using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class WeeklyLineupRepository(IDatabaseContext context) : IWeeklyLineupRepository
{
    public Task<bool> AddWeeklyLineupBowler(WeeklyLineup weeklyLineup) => context.AddItemAsync(weeklyLineup);
    public Task<IEnumerable<WeeklyLineup>> GetAllWeeklyLineupsByWeekId(int id) => context.GetAllWithChildrenAsync<WeeklyLineup>(wl => wl.WeekId == id);
    public Task<IEnumerable<WeeklyLineup>> GetAllWeeklyLineups() => context.GetAllAsync<WeeklyLineup>();
    public Task<IEnumerable<WeeklyLineup>> GetWeeklyLineupsByWeekId(int weekId) => context.GetFilteredAsync<WeeklyLineup>(wl => wl.WeekId == weekId);
    public Task<bool> UpdateWeeklyLineup(WeeklyLineup weeklyLineup) => context.UpdateItemAsync(weeklyLineup);
}
