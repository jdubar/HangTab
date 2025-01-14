using HangTab.Models;

namespace HangTab.Services;
public interface IBowlerService
{
    Task<Bowler> GetBowlerById(int id);
    Task<IEnumerable<Bowler>> GetAllBowlers();
    Task<IEnumerable<Bowler>> GetRegularBowlers();
    Task<IEnumerable<Bowler>> GetSubstituteBowlers();
    Task<bool> AddBowler(Bowler bowler);
    Task<bool> DeleteBowler(int id); 
    Task<bool> UpdateBowler(Bowler bowler);
}
