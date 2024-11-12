using System.Linq.Expressions;

namespace HangTab.Data;
public interface IDatabaseService
{
    Task<bool> DropAllTables();
    Task<bool> ResetHangings();

    Task<IReadOnlyCollection<Bowler>> GetFilteredBowlers(Expression<Func<Bowler, bool>> predicate);

    Task<IEnumerable<BowlerViewModel>> GetMainBowlersByWeek(int week);
    Task<IEnumerable<WeekViewModel>> GetAllWeeks();
    Task<BusRideViewModel> GetBusRideViewModelByWeek(int week);
    Task<int> GetLatestWeek();
    //Task<int> GetBusRideTotal();

    Task<SeasonSettings> GetSeasonSettings();
    Task<bool> UpdateSeasonSettings(SeasonSettings viewModel);

    Task<bool> UpdateBowlerHangingsByWeek(BowlerViewModel viewModel, int week);
    Task<bool> UpdateBusRidesByWeek(BusRideViewModel viewModel, int week);
}
