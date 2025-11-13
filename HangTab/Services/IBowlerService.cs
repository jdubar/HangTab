using HangTab.Models;

namespace HangTab.Services;
public interface IBowlerService
{
    Task<Result> AddAsync(Bowler bowler);
    Task<Result<IEnumerable<Bowler>>> GetAllAsync();
    Task<Result<IEnumerable<Bowler>>> GetAllByWeekIdAsync(int id);
    Task<Result<Bowler>> GetByIdAsync(int id);
    Task<Result> RemoveAsync(int id);
    Task<Result> UpdateAsync(Bowler bowler);
}
