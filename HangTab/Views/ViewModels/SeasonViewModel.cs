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
        var lastWeek = allWeeks.OrderBy(w => w.WeekNumber).Last().WeekNumber;

        var season = new List<WeekViewModel>();
        for (var week = lastWeek; week >= 1; week--)
        {
            var bowlers = allWeeks.Where(w => w.WeekNumber == week)
                                  .Join(allBowlers,
                                        w => w.BowlerId,
                                        b => b.Id,
                                        (w, b) => new Bowler()
                                        {
                                            IsSub = b.IsSub,
                                            ImageUrl = b.ImageUrl,
                                            FirstName = b.FirstName,
                                            LastName = b.LastName,
                                            TotalHangings = w.Hangings
                                        });

            var busRide = await data.GetBusRideViewModelByWeek(week);
            var weekViewModel = new WeekViewModel()
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
}
