﻿using HangTab.Extensions;
using System.Linq.Expressions;

namespace HangTab.Data.Impl;
public class DatabaseService(IDatabaseContext context) : IDatabaseService
{
    public async Task<bool> AddBowler(Bowler bowler)
        => await context.AddItemAsync(bowler);

    public async Task<bool> DeleteBowler(int id)
        => await context.DeleteItemByIdAsync<Bowler>(id);

    public async Task<bool> DropAllTables()
        => await context.DropTableAsync<Bowler>()
        && await context.DropTableAsync<BowlerWeek>()
        && await context.DropTableAsync<BusRide>()
        && await context.DropTableAsync<BusRideWeek>();

    public async Task<IReadOnlyCollection<Bowler>> GetAllBowlers()
        => await context.GetAllAsync<Bowler>();

    public async Task<IReadOnlyCollection<Bowler>> GetFilteredBowlers(Expression<Func<Bowler, bool>> predicate)
        => await context.GetFilteredAsync(predicate);

    public async Task<BusRideViewModel> GetBusRideViewModelByWeek(int week)
    {
        var viewmodel = new BusRideViewModel();
        var busRides = await context.GetAllAsync<BusRide>();
        if (busRides.Count > 0)
        {
            viewmodel.BusRide = busRides.Last();
        }
        else
        {
            if (!await context.AddItemAsync(viewmodel.BusRide))
            {
                return new BusRideViewModel();
            }
        }

        var busRideWeeks = await context.GetFilteredAsync<BusRideWeek>(b => b.WeekNumber == week);
        if (busRideWeeks.Count > 0)
        {
            viewmodel.BusRideWeek = busRideWeeks.Last();
        }
        else
        {
            viewmodel.BusRideWeek.WeekNumber = week;
            if (!await context.AddItemAsync(viewmodel.BusRideWeek))
            {
                return new BusRideViewModel();
            }
        }

        return viewmodel;
    }

    public async Task<IEnumerable<BowlerViewModel>> GetMainBowlersByWeek(int week)
    {
        var mains = new List<BowlerViewModel>();
        var bowlers = await GetFilteredBowlers(b => !b.IsHidden);
        var lowestHangBowlers = bowlers.GetLowestHangBowlers();
        var bowlerWeeks = await context.GetFilteredAsync<BowlerWeek>(w => w.WeekNumber == week);
        foreach (var bowler in bowlers)
        {
            var bowlerWeek = bowlerWeeks.FirstOrDefault(w => w.BowlerId == bowler.Id)
                             ?? new BowlerWeek
                             {
                                 WeekNumber = week,
                                 BowlerId = bowler.Id,
                                 Hangings = 0
                             };

            var viewModel = new BowlerViewModel
            {
                Bowler = bowler,
                BowlerWeek = bowlerWeek,
                IsLowestHangs = lowestHangBowlers.Exists(b => b.Id == bowler.Id)
            };
            mains.Add(viewModel);
        }
        return mains;
    }

    public async Task<IEnumerable<WeekViewModel>> GetAllWeeks()
    {
        var latestWeek = await GetLatestWeek();
        if (latestWeek <= 1)
        {
            return new List<WeekViewModel>();
        }

        var allBowlers = await GetAllBowlers();
        if (allBowlers.Count < 1)
        {
            return new List<WeekViewModel>();
        }

        var allBowlerWeeks = await context.GetAllAsync<BowlerWeek>();
        if (allBowlerWeeks.Count < 1)
        {
            return new List<WeekViewModel>();
        }

        var season = new List<WeekViewModel>();
        for (var week = latestWeek - 1; week >= 1; week--)
        {
            var bowlers = allBowlerWeeks.Where(w => w.WeekNumber == week)
                .Join(allBowlers,
                    w => w.BowlerId,
                    b => b.Id,
                    (w, b) => new Bowler
                    {
                        Id = b.Id,
                        IsSub = b.IsSub,
                        ImageUrl = b.ImageUrl,
                        FirstName = b.FirstName,
                        LastName = b.LastName,
                        TotalHangings = w.Hangings
                    })
                .ToList();

            var busRide = await GetBusRideViewModelByWeek(week);
            var weekViewModel = new WeekViewModel
            {
                WeekNumber = week,
                Bowlers = bowlers,
                TotalBusRides = busRide.BusRideWeek.BusRides,
                TotalHangings = bowlers.Sum(w => w.TotalHangings)
            };
            season.Add(weekViewModel);
        }

        return season;
    }

    public async Task<int> GetBusRideTotal()
    {
        var busRide = await context.GetItemByIdAsync<BusRide>(1);
        return busRide.Total;
    }

    public async Task<int> GetLatestWeek()
    {
        var busRideWeeks = await context.GetAllAsync<BusRideWeek>();
        return busRideWeeks.Count > 0
            ? busRideWeeks.OrderBy(w => w.WeekNumber).Last().WeekNumber
            : 1;
    }

    public async Task<bool> IsBowlerExists(Bowler bowler)
    {
        return !string.IsNullOrEmpty(bowler.FirstName)
            && (await context.GetFilteredAsync<Bowler>(b => b.FirstName == bowler.FirstName
                                                            && b.LastName == bowler.LastName)).Count > 0;
    }

    public async Task<bool> ResetHangings()
    {
        try
        {
            var bowlers = await context.GetAllAsync<Bowler>();
            foreach (var bowler in bowlers)
            {
                bowler.TotalHangings = 0;
                if (!await UpdateBowler(bowler))
                {
                    return false;
                }
            }
            return await context.DropTableAsync<BowlerWeek>()
                && await context.DropTableAsync<BusRide>()
                && await context.DropTableAsync<BusRideWeek>();
        }
        catch (Exception)
        {
            return false;
        }
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
        return weeks.Count > 0
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
        return busRides.Count > 0
            ? await context.UpdateItemAsync(viewModel.BusRideWeek)
            : await context.AddItemAsync(viewModel.BusRideWeek);
    }
}
