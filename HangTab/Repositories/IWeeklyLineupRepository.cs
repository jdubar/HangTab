using HangTab.Models;

namespace HangTab.Repositories;
public interface IWeeklyLineupRepository
{
    Task<bool> AddWeeklyLineupBowler(WeeklyLineup weeklyLineup);
    Task<IEnumerable<WeeklyLineup>> GetWeeklyLineupsByWeekId(int weekId);
}
