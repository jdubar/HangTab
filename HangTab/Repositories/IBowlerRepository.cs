using HangTab.Models;

namespace HangTab.Repositories;
public interface IBowlerRepository
{
    Task<bool> AddBowlerAsync(Bowler bowler);
    Task<IEnumerable<Bowler>> GetAllBowlersByWeekIdAsync(int id);
    Task<IEnumerable<Bowler>> GetAllBowlersAsync();
    Task<Bowler> GetBowlerByIdAsync(int id);
    Task<bool> RemoveBowlerAsync(int id);
    Task<bool> UpdateBowlerAsync(Bowler bowler);
}
