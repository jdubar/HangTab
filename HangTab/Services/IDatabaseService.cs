using HangTab.Models;

using System.Linq.Expressions;

namespace HangTab.Services;
public interface IDatabaseService
{
    Task<bool> DropAllTables();

    Task<IEnumerable<Bowler>> GetAllBowlers();
    Task<IEnumerable<Bowler>> GetFilteredBowlers(Expression<Func<Bowler, bool>> predicate);

    Task<int> GetWorkingWeek();

    Task<bool> AddBowler(Bowler bowler);
    Task<bool> DeleteBowler(int id);
    Task<bool> IsBowlerExists(Bowler bowler);
    Task<bool> UpdateBowler(Bowler bowler);

    Task<Week> GetLatestWeek();
    Task<int> GetTotalBusRides();
    Task<bool> UpdateWeek(Week week);
    Task<bool> UpdateTotalHangs(int hangs);
    Task<bool> UpdateAllBowlers(IEnumerable<Bowler> bowlers);
    Task<Week> StartNewWeek();
}
