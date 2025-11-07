using HangTab.Models;

namespace HangTab.Services;
public interface IWeekService
{
    Task<Week> GetWeekByIdAsync(int id);
    Task<IEnumerable<Week>> GetAllWeeksAsync();
    Task<Week> CreateWeekAsync(int weekNumber);
    Task UpdateWeekAsync(Week week);
}
