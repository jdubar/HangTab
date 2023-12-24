using HangTab.Models;
using HangTab.ViewModels;

namespace HangTab.Services;
public interface IDatabaseService
{
    Task<BusRideViewModel> GetLatestBusRideWeek(int week);
    Task<int> SetWorkingWeek();
    Task UpdateBowler(Bowler viewModel);
    Task UpdateBowlerHangingsByWeek(BowlerViewModel viewModel, int week);
    Task UpdateBusRidesByWeek(BusRideViewModel viewModel, int week);
}
