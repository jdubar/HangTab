using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class WeeklyLineupService(IWeeklyLineupRepository weeklyLineupRepository) : IWeeklyLineupService
{
    public Task<bool> AddWeeklyLineupBowler(WeeklyLineup weeklyLineup) => weeklyLineupRepository.AddWeeklyLineupBowler(weeklyLineup);
    public Task<IEnumerable<WeeklyLineup>> GetWeeklyLineupsByWeekId(int weekId) => weeklyLineupRepository.GetWeeklyLineupsByWeekId(weekId);
}
