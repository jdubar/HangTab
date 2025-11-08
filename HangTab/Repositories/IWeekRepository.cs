using HangTab.Models;

namespace HangTab.Repositories;
public interface IWeekRepository
{
    Task<Week> CreateAsync(int weekNumber = 1);
    Task<IEnumerable<Week>> GetAllAsync();
    Task<Week> GetByIdAsync(int id);
    Task UpdateAsync(Week week);
}
