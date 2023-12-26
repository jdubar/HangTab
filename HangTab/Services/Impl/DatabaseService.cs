using HangTab.Data;
using HangTab.Models;
using HangTab.ViewModels;

namespace HangTab.Services.Impl;
public class DatabaseService(IDatabaseContext context) : IDatabaseService
{
    public async Task<IEnumerable<BowlerWeek>> GetAllWeeks() =>
        await context.GetAllAsync<BowlerWeek>();

    public async Task<BusRideViewModel> GetLatestBusRideWeek(int week)
    {
        var viewmodel = new BusRideViewModel();
        var busRides = await context.GetAllAsync<BusRide>();
        if (busRides.Any())
        {
            viewmodel.BusRide = busRides.Last();
        }
        else
        {
            _ = await context.AddItemAsync(viewmodel.BusRide);
        }
        var weeks = await context.GetFilteredAsync<BusRideWeek>(b => b.WeekNumber == week);
        if (weeks.Any())
        {
            viewmodel.BusRideWeek = weeks.Last();
        }
        else
        {
            viewmodel.BusRideWeek.BusRideId = viewmodel.BusRide.Id;
            viewmodel.BusRideWeek.WeekNumber = week;
            _ = await context.AddItemAsync(viewmodel.BusRideWeek);
        }
        return viewmodel;
    }

    public async Task<IEnumerable<Bowler>> GetActiveBowlers() =>
        await context.GetFilteredAsync<Bowler>(b => !b.IsHidden);

    public async Task<IEnumerable<Bowler>> GetSwitchBowlers(int id) =>
        await context.GetFilteredAsync<Bowler>(b => b.Id != id && b.IsHidden);

    public async Task<IEnumerable<BowlerWeek>> GetWeeksByWeek(int week) =>
        await context.GetFilteredAsync<BowlerWeek>(w => w.WeekNumber == week);

    public async Task<int> SetWorkingWeek()
    {
        var allWeeks = await context.GetAllAsync<BowlerWeek>();
        return allWeeks is not null && allWeeks.Any()
            ? allWeeks.OrderBy(w => w.WeekNumber).Last().WeekNumber
            : 1;
    }

    public async Task UpdateBowler(Bowler viewModel) =>
        _ = await context.UpdateItemAsync(viewModel);

    public async Task UpdateBowlerHangingsByWeek(BowlerViewModel viewModel, int week)
    {
        _ = await context.UpdateItemAsync(viewModel.Bowler);

        var weeks = await context.GetFilteredAsync<BowlerWeek>(w => w.WeekNumber == week
                                                                    && w.BowlerId == viewModel.Bowler.Id);
        _ = weeks is not null && weeks.Any()
            ? await context.UpdateItemAsync(viewModel.BowlerWeek)
            : await context.AddItemAsync(viewModel.BowlerWeek);
    }

    public async Task UpdateBusRidesByWeek(BusRideViewModel viewModel, int week)
    {
        _ = await context.UpdateItemAsync(viewModel.BusRide);
        var busRides = await context.GetFilteredAsync<BusRideWeek>(b => b.WeekNumber == week);
        _ = busRides is not null && busRides.Any()
            ? await context.UpdateItemAsync(viewModel.BusRideWeek)
            : await context.AddItemAsync(viewModel.BusRideWeek);
    }
}
