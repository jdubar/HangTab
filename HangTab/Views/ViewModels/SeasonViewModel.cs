using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
public partial class SeasonViewModel(IDatabaseService data) : BaseViewModel
{
    public ObservableRangeCollection<Week> AllWeeks { get; set; } = [];

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            var weeks = await data.GetAllBowlerWeeks();

            AllWeeks.Clear();

            if (weeks.Any())
            {
                AllWeeks.AddRange(await LoadBowlers(weeks));
            }
        }, "");
    }

    private async Task<List<Week>> LoadBowlers(IEnumerable<BowlerWeek> allWeeks)
    {
        var lastWeek = allWeeks.OrderBy(w => w.WeekNumber).Last().WeekNumber;
        var collection = new List<Week>();
        for (var i = lastWeek; i >= 1; i--)
        {
            var totalHangs = 0;
            var workingWeeks = allWeeks.Where(w => w.WeekNumber == i);
            foreach (var week in workingWeeks)
            {
                totalHangs += week.Hangings;
            }
            var busRide = await data.GetLatestBusRide(i);
            var viewModel = new Week()
            {
                WeekNumber = i,
                TotalBusRides = busRide.BusRideWeek.BusRides,
                TotalHangings = totalHangs
            };
            collection.Add(viewModel);
        }
        return collection;
    }
}
