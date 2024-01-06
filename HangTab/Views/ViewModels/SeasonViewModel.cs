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
        var allBusRides = await data.GetAllBusRideWeeks();
        var lastWeek = allBusRides.OrderBy(w => w.WeekNumber).Last().WeekNumber;
        var collection = new List<WeekViewModel>();
        for (var i = lastWeek - 1; i >= 1; i--)
        {
            var totalHangs = 0;
            var bowlers = new List<Bowler>();
            var workingWeeks = allWeeks.Where(w => w.WeekNumber == i).OrderByDescending(b => b.Hangings);
            foreach (var week in workingWeeks)
            {
                var bowler = new Bowler
                {
                    IsSub = allBowlers.First(b => b.Id == week.BowlerId).IsSub,
                    ImageUrl = allBowlers.First(b => b.Id == week.BowlerId).ImageUrl,
                    FirstName = allBowlers.First(b => b.Id == week.BowlerId).FirstName,
                    LastName = allBowlers.First(b => b.Id == week.BowlerId).LastName,
                    TotalHangings = week.Hangings
                };
                bowlers.Add(bowler);
                totalHangs += week.Hangings;
            }

            var busRide = await data.GetLatestBusRide(i);
            var viewModel = new WeekViewModel()
            {
                WeekNumber = i,
                TotalBusRides = busRide.BusRideWeek.BusRides,
                TotalHangings = totalHangs,
                Bowlers = bowlers
            };
            collection.Add(viewModel);
        }
        return collection;
    }
}
