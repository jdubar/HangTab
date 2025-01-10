using HangTab.Models;

namespace HangTab.Repositories;
public interface IWeekRepository
{
    Task<Week> GetWeek(int id);
    Task<IEnumerable<Week>> GetWeeks();
    Task<Week> GetWeekByWeekNumber(int weekNumber);
}
