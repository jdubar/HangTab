using HangTab.Models;

namespace HangTab.Services;
public interface IWeekService
{
    Task<Result<Week>> AddAsync(int weekNumber = 1);
    Task<Result<IEnumerable<Week>>> GetAllAsync();
    Task<Result<Week>> GetByIdAsync(int id);
    Task<Result> UpdateAsync(Week week);
}
