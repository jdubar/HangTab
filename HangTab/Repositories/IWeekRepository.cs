using HangTab.Models;

namespace HangTab.Repositories;
public interface IWeekRepository
{
    Task<Week> GetWeekById(int id);
    Task<IEnumerable<Week>> GetAllWeeks();
    Task<Week> CreateWeek(int weekNumber = 1);
    Task UpdateWeek(Week week);
}
