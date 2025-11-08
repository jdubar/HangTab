using HangTab.Models;

namespace HangTab.Services;
public interface IBowlerService
{
    Task<bool> AddAsync(Bowler bowler);
    Task<IEnumerable<Bowler>> GetAllAsync();
    Task<IEnumerable<Bowler>> GetAllByWeekIdAsync(int id);
    Task<Bowler> GetByIdAsync(int id);
    Task<bool> RemoveAsync(int id);
    Task<bool> UpdateAsync(Bowler bowler);
}
