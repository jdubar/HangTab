using HangTab.Models;

namespace HangTab.Services;
public interface IWeeklyLineupService
{
    Task<bool> AddWeeklyLineupBowler(WeeklyLineup weeklyLineup);
    Task<IEnumerable<WeeklyLineup>> GetAllWeeklyLineupsByWeekId(int id);
    Task<IEnumerable<WeeklyLineup>> GetAllWeeklyLineups();
    Task<IEnumerable<WeeklyLineup>> GetWeeklyLineupsByWeekId(int weekId);
    Task<bool> UpdateWeeklyLineup(WeeklyLineup weeklyLineup);
}
