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

        var lowestHangIds = await GetLowestHangsIds();
        week.Bowlers = await GetFilteredBowlers(b => !b.IsHidden);
        foreach (var bowler in week.Bowlers)
        {
            bowler.IsLowestHangs = lowestHangIds.Contains(bowler.Id);
        }
        return week;
    }

    public async Task<IEnumerable<int>> GetLowestHangsIds()
    {
        var bowlers = await context.GetAllAsync<Bowler>();
        return bowlers.Where((x) => !x.IsSub && x.TotalHangings == bowlers.Min(y => y.TotalHangings)).Select(b => b.Id);
    }

    public async Task<int> GetTotalBusRides()
    {
        var busrides = await context.GetAllAsync<BusRide>();
        return busrides is not null && busrides.Any()
            ? busrides.First().TotalBusRides
            : 0;
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

    public async Task<bool> UpdateAllBowlers(IEnumerable<Bowler> bowlers)
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

    public async Task<bool> UpdateTotalHangs(int hangs)
    {
        var busrides = await context.GetFilteredAsync<BusRide>(b => b.Id == 1);
        var busride = busrides.First();
        busride.TotalBusRides += hangs;
        return await context.UpdateItemAsync(busride);
    }

    public async Task<Week> StartNewWeek()
    {
        var week = new Week()
        {
            Bowlers = await GetAllBowlers()
        };
        return await context.AddItemAsync(week)
            ? week
            : new();
    }
}
