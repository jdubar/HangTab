﻿using System.Collections.ObjectModel;
using System.Linq.Expressions;
using HangTab.Data;
using HangTab.Models;
using HangTab.ViewModels;

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

    public async Task<IEnumerable<Bowler>> GetFilteredBowlers(Expression<Func<Bowler, bool>> predicate) =>
        await context.GetFilteredAsync(predicate);

    public async Task<IEnumerable<BowlerWeek>> GetAllBowlerWeeks() =>
        await context.GetAllAsync<BowlerWeek>();

    public async Task<IEnumerable<BowlerWeek>> GetBowlerWeeksByWeek(int week) =>
        await context.GetFilteredAsync<BowlerWeek>(w => w.WeekNumber == week);

    public async Task<BusRideViewModel> GetBusRideViewModelByWeek(int week)
    {
        var viewmodel = new BusRideViewModel();
        var data = await context.GetAllAsync<BusRide>();
        if (data is null)
        {
            return viewmodel;
        }

        var busRides = data.ToList();
        if (busRides.Any())
        {
            viewmodel.BusRide = Enumerable.Last(busRides);
        }
        else
        {
            if (!await context.AddItemAsync(viewmodel.BusRide))
            {
                return new BusRideViewModel();
            }
        }
        var weeks = await context.GetFilteredAsync<BusRideWeek>(b => b.WeekNumber == week);
        if (weeks is null)
        {
            viewmodel.BusRideWeek.WeekNumber = week;
            if (!await context.AddItemAsync(viewmodel.BusRideWeek))
            {
                return new BusRideViewModel();
            }

            return viewmodel;
        }

        viewmodel.BusRideWeek = weeks.Last();
        return viewmodel;
    }

    public async Task<IEnumerable<BowlerViewModel>> GetMainBowlersByWeek(int week)
    {
        var mainBowlers = new List<BowlerViewModel>();
        var bowlers = await GetFilteredBowlers(b => !b.IsHidden);
        var bowlerWeeks = await GetBowlerWeeksByWeek(week);
        var lowestHangBowlers = bowlers.Where(b => !b.IsSub
                                                   && b.TotalHangings == bowlers.Where(bowler => !bowler.IsSub).Min(bowler => bowler.TotalHangings)).ToList();

        foreach (var bowler in bowlers)
        {
            var bowlerWeek = bowlerWeeks.FirstOrDefault(w => w.BowlerId == bowler.Id);
            bowlerWeek ??= new BowlerWeek
            {
                WeekNumber = week,
                BowlerId = bowler.Id,
                Hangings = 0
            };

            var viewModel = new BowlerViewModel
            {
                Bowler = bowler,
                BowlerWeek = bowlerWeek,
                IsLowestHangs = Enumerable.Any(lowestHangBowlers, b => b.Id == bowler.Id)
            };
            mainBowlers.Add(viewModel);
        }
        return mainBowlers;
    }

    public async Task<IEnumerable<WeekViewModel>> GetAllWeeks()
    {
        var allBowlers = await GetAllBowlers();
        var allBowlerWeeks = await GetAllBowlerWeeks();

        var season = new List<WeekViewModel>();
        if (allBowlerWeeks.Any() && allBowlers.Any())
        {
            var lastWeek = await GetLatestWeek();
            for (var week = lastWeek - 1; week >= 1; week--)
            {
                var bowlers = allBowlerWeeks.Where(w => w.WeekNumber == week)
                                            .Join(allBowlers,
                                                  w => w.BowlerId,
                                                  b => b.Id,
                                                  (w, b) => new Bowler
                                                  {
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
        }
        return season;
    }

    public async Task<int> GetLatestWeek()
    {
        var data = await context.GetAllAsync<BusRideWeek>();
        if (data is null)
        {
            return 1;
        }

        var allWeeks = data.ToList();
        return allWeeks.Any()
            ? allWeeks.OrderBy(w => w.WeekNumber).Last().WeekNumber
            : 1;
    }

    public async Task<SeasonSettings> GetSeasonSettings()
    {
        var data = await context.GetAllAsync<SeasonSettings>();
        if (data is null)
        {
            return new SeasonSettings();
        }

        var settings = data.ToList();
        return settings.Any()
            ? Enumerable.First(settings)
            : new SeasonSettings();
    }

    public async Task<bool> IsBowlerExists(Bowler bowler)
    {
        var find = await context.GetFilteredAsync<Bowler>(b => b.FirstName == bowler.FirstName
                                                               && b.LastName == bowler.LastName);
        return find.Any();
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

    public async Task<bool> UpdateSeasonSettings(SeasonSettings viewModel)
    {
        var settings = await context.GetAllAsync<SeasonSettings>();
        return settings is not null && settings.Any()
            ? await context.UpdateItemAsync(viewModel)
            : await context.AddItemAsync(viewModel);
    }
}
