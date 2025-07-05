using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Mappers;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels.Base;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class SeasonSummaryViewModel(
    IBowlerService bowlerService,
    IWeekService weekService,
    IMapper<IEnumerable<Bowler>, IEnumerable<BowlerListItemViewModel>> mapper) : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<BowlerListItemViewModel> _bowlers = [];

    [ObservableProperty]
    private WeekListItemViewModel? _bestHangingWeek;

    [ObservableProperty]
    private WeekListItemViewModel? _worstHangingWeek;

    [ObservableProperty]
    private WeekListItemViewModel? _bestBusRideWeek;

    public override async Task LoadAsync()
    {
        if (Bowlers.Count == 0)
        {
            await Loading(async () =>
            {
                await GetBowlers();
                await GetWeeks();
            });
        }
    }

    private async Task GetBowlers()
    {
        var allBowlers = await bowlerService.GetAllBowlers();
        if (!allBowlers.Any())
        {
            return;
        }

        var lowestHangCount = allBowlers.Min(b => b.HangCount);
        var bowlers = allBowlers.Where(b => b.HangCount == lowestHangCount);

        Bowlers = mapper.Map(bowlers).ToObservableCollection();
    }

    private async Task GetWeeks()
    {
        var allWeeks = await weekService.GetAllWeeks();
        if (!allWeeks.Any())
        {
            return;
        }

        var weeks = allWeeks.Select(w => new WeekListItemViewModel(w.Id, w.BusRides, w.Number, w.Bowlers.Sum(b => b.HangCount)))
                            .OrderByDescending(w => w.HangCount);

        BestHangingWeek = weeks.First();
        WorstHangingWeek = weeks.Last();

        BestBusRideWeek = weeks.OrderByDescending(w => w.BusRides).First();
    }
}
