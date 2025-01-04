using HangTab.Models;

namespace HangTab.Repositories;
public interface IBowlerRepository
{
    Task<IEnumerable<Bowler>> GetBowlers();
    Task<Bowler> GetBowler(int id);
    Task<bool> AddBowler(Bowler bowler);
    Task<bool> DeleteBowler(int id);
    Task<bool> UpdateBowler(Bowler bowler);
}
