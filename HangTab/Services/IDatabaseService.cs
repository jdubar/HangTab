using System.Linq.Expressions;

namespace HangTab.Services;
public interface IDatabaseService
{
    Task<bool> DropAllTables();
    Task<bool> ResetHangings();

    Task<IReadOnlyCollection<Bowler>> GetAllBowlers();
    Task<IReadOnlyCollection<Bowler>> GetFilteredBowlers(Expression<Func<Bowler, bool>> predicate);

    Task<IEnumerable<BowlerViewModel>> GetMainBowlersByWeek(int week);
    Task<IEnumerable<WeekViewModel>> GetAllWeeks();
    Task<BusRideViewModel> GetBusRideViewModelByWeek(int week);
    Task<int> GetLatestWeek();

    Task<SeasonSettings> GetSeasonSettings();
    Task<bool> UpdateSeasonSettings(SeasonSettings viewModel);

    Task<bool> AddBowler(Bowler bowler);
    Task<bool> DeleteBowler(int id);
    Task<bool> IsBowlerExists(Bowler bowler);
    Task<bool> UpdateBowler(Bowler bowler);

    Task<bool> UpdateBowlerHangingsByWeek(BowlerViewModel viewModel, int week);
    Task<bool> UpdateBusRidesByWeek(BusRideViewModel viewModel, int week);
}
