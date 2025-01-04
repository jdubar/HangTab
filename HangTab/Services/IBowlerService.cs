using HangTab.Models;

namespace HangTab.Services;
public interface IBowlerService
{
    Task<bool> AddBowler(Bowler bowler);
    Task<Bowler> GetBowler(int id);
    Task<IEnumerable<Bowler>> GetBowlers();
    Task<bool> DeleteBowler(int id); 
    Task<bool> UpdateBowler(Bowler bowler);
}
