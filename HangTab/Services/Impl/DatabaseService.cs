using HangTab.Data;
using HangTab.Models;

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
        && await context.DropTableAsync<BusRide>();

    public async Task<IEnumerable<Bowler>> GetAllBowlers() =>
        await context.GetAllAsync<Bowler>();

    public async Task<IEnumerable<Bowler>> GetFilteredBowlers(Expression<Func<Bowler, bool>> predicate) =>
        await context.GetFilteredAsync(predicate);

    public async Task<Week> GetLatestWeek()
    {
        var weeks = await context.GetAllAsync<Week>();
        var week = weeks is not null && weeks.Any()
                 ? weeks.OrderBy(w => w.WeekNumber).Last()
                 : new();

        week.Bowlers = await GetFilteredBowlers(b => !b.IsHidden);
        return week;
    }

    public async Task<int> GetTotalBusRides()
    {
        var busrides = await context.GetAllAsync<BusRide>();
        return busrides is not null && busrides.Any()
            ? busrides.First().TotalBusRides
            : 0;
    }

    public async Task<IEnumerable<Week>> GetAllWeeks()
    {
        var allWeeks = await context.GetAllAsync<Week>();
        return allWeeks.OrderByDescending(w => w.WeekNumber);
    }

    public async Task<int> GetWorkingWeek()
    {
        var allWeeks = await context.GetAllAsync<BusRideWeek>();
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

    public async Task<bool> SaveAllBowlerData(IEnumerable<Bowler> bowlers)
    {
        foreach (var bowler in bowlers)
        {
            if (!await UpdateBowler(bowler))
            {
                return false;
            }
        }
        return true;
    }

    public async Task<bool> UpdateWeek(Week week)
    {
        var weeks = await context.GetFilteredAsync<Week>(b => b.WeekNumber == week.WeekNumber);
        return weeks is not null && weeks.Any()
            ? await context.UpdateItemAsync(week)
            : await context.AddItemAsync(week);
    }

    //public async Task<bool> UpdateTotalHangs(int hangs)
    //{
    //    var busride = new BusRide();
    //    var busrides = await context.GetFilteredAsync<BusRide>(b => b.Id == 1);
    //    if (!busrides.Any())
    //    {
    //        await context.AddItemAsync(busride);
    //    }
    //    else
    //    {
    //        busride = busrides.First();
    //    }
    //    busride.TotalBusRides += hangs;
    //    return await context.UpdateItemAsync(busride);
    //}

    public async Task<bool> IncrementTotalHangs()
    {
        var busride = new BusRide();
        var busrides = await context.GetAllAsync<BusRide>();
        if (busrides.Any())
        {
            busride = busrides.First();
        }
        else
        {
            if (!await context.AddItemAsync(busride))
            {
                return false;
            }
        }
        busride.TotalBusRides++;
        return await context.UpdateItemAsync(busride);
    }

    public async Task<Week> StartNewWeek()
    {
        var bowlers = await GetFilteredBowlers(b => !b.IsHidden);
        foreach (var bowler in bowlers)
        {
            bowler.WeekHangings = 0;
        }
        var week = new Week()
        {
            Bowlers = bowlers,
            BusRides = 0,
            TotalHangingsForTheWeek = 0
        };
        return await context.AddItemAsync(week)
            ? week
            : new();
    }
}
