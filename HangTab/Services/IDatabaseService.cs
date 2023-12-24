using HangTab.Models;
using HangTab.ViewModels;

namespace HangTab.Services;
public interface IDatabaseService
{
    Task<IEnumerable<BowlerWeek>> GetAllWeeks();
    Task<BusRideViewModel> GetLatestBusRideWeek(int week);
    Task<IEnumerable<Bowler>> GetMainBowlers();
    Task<IEnumerable<Bowler>> GetSwitchBowlers(int id);
    Task<IEnumerable<BowlerWeek>> GetWeeksByWeek(int week);
    Task<int> SetWorkingWeek();
    Task UpdateBowler(Bowler viewModel);
    Task UpdateBowlerHangingsByWeek(BowlerViewModel viewModel, int week);
    Task UpdateBusRidesByWeek(BusRideViewModel viewModel, int week);
}
