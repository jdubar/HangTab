using HangTab.Models;

namespace HangTab.Repositories;
public interface IBowlerRepository
{
    Task<bool> AddAsync(Bowler bowler);
    Task<IEnumerable<Bowler>> GetAllByWeekIdAsync(int id);
    Task<IEnumerable<Bowler>> GetAllAsync();
    Task<Bowler> GetByIdAsync(int id);
    Task<bool> RemoveAsync(int id);
    Task<bool> UpdateAsync(Bowler bowler);
}
