using HangTab.Models;

namespace HangTab.Repositories;
public interface IWeeklyLineupRepository
{
    Task<bool> AddWeeklyLineupBowler(WeeklyLineup weeklyLineup);
    Task<IEnumerable<WeeklyLineup>> GetAllWeeklyLineupsByWeekId(int id);
    Task<IEnumerable<WeeklyLineup>> GetAllWeeklyLineups();
    Task<WeeklyLineup> GetWeeklyLineupById(int id);
    Task<IEnumerable<WeeklyLineup>> GetWeeklyLineupsByWeekId(int weekId);
    Task<bool> UpdateWeeklyLineup(WeeklyLineup weeklyLineup);
}
