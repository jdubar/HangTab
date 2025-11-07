using HangTab.Models;

namespace HangTab.Repositories;
public interface IWeekRepository
{
    Task<Week> GetWeekByIdAsync(int id);
    Task<IEnumerable<Week>> GetAllWeeksAsync();
    Task<Week> CreateWeekAsync(int weekNumber = 1);
    Task UpdateWeekAsync(Week week);
}
