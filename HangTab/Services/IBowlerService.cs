using HangTab.Models;

namespace HangTab.Services;
public interface IBowlerService
{
    Task<bool> AddBowler(Bowler bowler);
    Task<IEnumerable<Bowler>> GetAllBowlersByWeekId(int id);
    Task<IEnumerable<Bowler>> GetAllBowlers();
    Task<Bowler> GetBowlerById(int id);
    Task<IEnumerable<Bowler>> GetBowlersByWeekId(int weekId);
    Task<bool> UpdateBowler(Bowler bowler);
}
