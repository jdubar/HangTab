using HangTab.Models;

namespace HangTab.Services;
public interface IWeeklyLineupService
{
    Task<bool> AddWeeklyLineupBowler(WeeklyLineup weeklyLineup);
    Task<IEnumerable<WeeklyLineup>> GetWeeklyLineupsByWeekId(int weekId);
}
