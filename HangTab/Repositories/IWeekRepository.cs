using HangTab.Models;

namespace HangTab.Repositories;
public interface IWeekRepository
{
    Task<Week> GetWeek(int id);
    Task<IEnumerable<Week>> GetWeeks();
    Task<Week> GetWeekById(int id);
    Task<Week> CreateWeek(int weekNumber);
    Task UpdateWeek(Week week);
}
