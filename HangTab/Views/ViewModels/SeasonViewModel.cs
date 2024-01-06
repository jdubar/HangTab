using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
public partial class SeasonViewModel(IDatabaseService data,
                                     IShellService shell) : BaseViewModel
{
    public ObservableRangeCollection<WeekViewModel> AllWeeks { get; set; } = [];

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            var weeks = await data.GetAllBowlerWeeks();

            AllWeeks.Clear();

            if (weeks.Any())
            {
                AllWeeks.AddRange(await LoadSeason(weeks));
            }
        }, "");
    }

    [RelayCommand]
    private async Task ShowWeekDetailsAsync(WeekViewModel week) =>
        await shell.GoToPageWithData(nameof(WeekDetailsPage), week);

    private async Task<List<WeekViewModel>> LoadSeason(IEnumerable<BowlerWeek> allWeeks)
    {
        var allBowlers = await data.GetAllBowlers();
        var lastSavedWeek = data.GetWorkingWeek().Result;

        var season = new List<WeekViewModel>();
        for (var savedWeek = lastSavedWeek - 1; savedWeek >= 1; savedWeek--)
        {
            var totalHangs = 0;
            var bowlers = new List<Bowler>();
            var workingWeeks = allWeeks.Where(bw => bw.WeekNumber == savedWeek).OrderByDescending(b => b.Hangings);
            foreach (var week in workingWeeks)
            {
                var bowler = allBowlers.First(b => b.Id == week.BowlerId);
                var bowlerModel = new Bowler
                {
                    IsSub = bowler.IsSub,
                    ImageUrl = bowler.ImageUrl,
                    FirstName = bowler.FirstName,
                    LastName = bowler.LastName,
                    TotalHangings = week.Hangings
                };
                bowlers.Add(bowlerModel);
                totalHangs += week.Hangings;
            }

            var busRide = await data.GetBusRideViewModelByWeek(savedWeek);
            var weekViewModel = new WeekViewModel()
            {
                WeekNumber = savedWeek,
                TotalBusRides = busRide.BusRideWeek.BusRides,
                TotalHangings = totalHangs,
                Bowlers = bowlers
            };
            season.Add(weekViewModel);
        }
        return season;
    }
}
