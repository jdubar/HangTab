using HangTab.Data;
using HangTab.Models;
using HangTab.ViewModels;

using System.Linq.Expressions;

namespace HangTab.Services.Impl;
public class DatabaseService(IDatabaseContext context) : IDatabaseService
{
    public async Task<bool> AddBowler(Bowler bowler) =>
        await context.AddItemAsync(bowler);

    public async Task<bool> DeleteBowler(int id) =>
        await context.DeleteItemByIdAsync<Bowler>(id);

    public async Task<bool> DropAllTables() =>
        await context.DropTableAsync<Bowler>()
        && await context.DropTableAsync<BowlerWeek>()
        && await context.DropTableAsync<BusRide>()
        && await context.DropTableAsync<BusRideWeek>();

    public async Task<IEnumerable<Bowler>> GetAllBowlers() =>
        await context.GetAllAsync<Bowler>();

    public async Task<IEnumerable<BowlerWeek>> GetAllBowlerWeeks() =>
        await context.GetAllAsync<BowlerWeek>();

    public async Task<IEnumerable<Bowler>> GetFilteredBowlers(Expression<Func<Bowler, bool>> predicate) =>
        await context.GetFilteredAsync(predicate);

    public async Task<IEnumerable<BowlerWeek>> GetFilteredBowlerWeeks(int week) =>
        await context.GetFilteredAsync<BowlerWeek>(w => w.WeekNumber == week);

    public async Task<BusRideViewModel> GetLatestBusRide(int week)
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

    public async Task<IEnumerable<Bowler>> GetLowestHangs()
    {
        var bowlers = await context.GetAllAsync<Bowler>();
        return bowlers.Where((x) => !x.IsSub && x.TotalHangings == bowlers.Min(y => y.TotalHangings));
    }

    public async Task<int> GetWorkingWeek()
    {
        var allWeeks = await context.GetAllAsync<BowlerWeek>();
        return allWeeks is not null && allWeeks.Any()
            ? allWeeks.OrderBy(w => w.WeekNumber).Last().WeekNumber
            : 1;
    }
    public async Task<bool> IsBowlerExists(Bowler bowler)
    {
        var find = await context.GetFilteredAsync<Bowler>(b => b.FirstName == bowler.FirstName
                                                               && b.LastName == bowler.LastName);
        return find.Any();
    }

    public async Task<bool> UpdateBowler(Bowler bowler) =>
        await context.UpdateItemAsync(bowler);

    public async Task<bool> UpdateBowlerHangingsByWeek(BowlerViewModel viewModel, int week)
    {
        if (!await context.UpdateItemAsync(viewModel.Bowler))
        {
            return false;
        }

        var weeks = await context.GetFilteredAsync<BowlerWeek>(w => w.WeekNumber == week
                                                                    && w.BowlerId == viewModel.Bowler.Id);
        return weeks is not null && weeks.Any()
            ? await context.UpdateItemAsync(viewModel.BowlerWeek)
            : await context.AddItemAsync(viewModel.BowlerWeek);
    }

    public async Task<bool> UpdateBusRidesByWeek(BusRideViewModel viewModel, int week)
    {
        if (!await context.UpdateItemAsync(viewModel.BusRide))
        {
            return false;
        }
        var busRides = await context.GetFilteredAsync<BusRideWeek>(b => b.WeekNumber == week);
        return busRides is not null && busRides.Any()
            ? await context.UpdateItemAsync(viewModel.BusRideWeek)
            : await context.AddItemAsync(viewModel.BusRideWeek);
    }
}
