using HangTab.Models;

namespace HangTab.Services;
public interface IWeekService
{
    Task<Week> GetWeek(int id);
    Task<IEnumerable<Week>> GetAllWeeks();
    Task<Week> GetWeekById(int weekNumber);
    Task<Week> CreateWeek(int weekNumber);
    Task UpdateWeek(Week week);
}
