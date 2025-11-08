using HangTab.Models;

namespace HangTab.Services;
public interface IWeekService
{
    Task<Week> CreateAsync(int weekNumber);
    Task<IEnumerable<Week>> GetAllAsync();
    Task<Week> GetByIdAsync(int id);
    Task UpdateAsync(Week week);
}
