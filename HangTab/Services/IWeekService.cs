using HangTab.Models;

namespace HangTab.Services;
public interface IWeekService
{
    Task<Week> GetWeekById(int id);
    Task<IEnumerable<Week>> GetAllWeeks();
    Task<Week> CreateWeek(int weekNumber);
    Task UpdateWeek(Week week);
}
