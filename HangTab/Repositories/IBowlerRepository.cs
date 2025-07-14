using HangTab.Models;

namespace HangTab.Repositories;
public interface IBowlerRepository
{
    Task<bool> AddBowler(Bowler bowler);
    Task<IEnumerable<Bowler>> GetAllBowlersByWeekId(int id);
    Task<IEnumerable<Bowler>> GetAllBowlers();
    Task<Bowler> GetBowlerById(int id);
    Task<bool> UpdateBowler(Bowler bowler);
}
