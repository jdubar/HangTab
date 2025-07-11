using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Extensions;
using HangTab.Mappers;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels.Base;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class SeasonSummaryViewModel(
    IPersonService personService,
    IWeekService weekService,
    IMapper<IEnumerable<Person>, IEnumerable<BowlerListItemViewModel>> mapper) : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<BowlerListItemViewModel> _bowlers = [];

    [ObservableProperty]
    private int _bestHangWeekNumber;

    [ObservableProperty]
    private int _bestHangCount;

    [ObservableProperty]
    private int _worstHangWeekNumber;

    [ObservableProperty]
    private int _worstHangCount;

    [ObservableProperty]
    private int _bestBusRideWeekNumber;

    [ObservableProperty]
    private int _bestBusRideCount;

    [ObservableProperty]
    private string _bowlerListHeader;

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
        var people = await personService.GetRegulars();
        if (!people.Any())
        {
            return;
        }

        var allWeeks = await weekService.GetAllWeeks();
        if (!allWeeks.Any())
        {
            return;
        }

        var bowlers = mapper.Map(people.OrderBy(b => b.Name)).ToList();
        bowlers.SetBowlerHangSumByWeeks(allWeeks);

        Bowlers = bowlers.Where(b => b.HangCount == bowlers.Min(b => b.HangCount)).ToObservableCollection();
        var s = Bowlers.Count > 1 ? "s" : "";
        BowlerListHeader = $"Bowler{s} with the least hangs";
    }

    private async Task GetWeeks()
    {
        var allWeeks = await weekService.GetAllWeeks();
        if (!allWeeks.Any())
        {
            return;
        }

        var weeks = allWeeks
            .Select(w => new WeekListItemViewModel(
                w.Id,
                w.Number,
                w.BusRides,
                w.Bowlers.Sum(b => b.HangCount)))
            .OrderByDescending(w => w.HangCount);

        var bestHangingWeek = weeks.First();
        BestHangWeekNumber = bestHangingWeek.Number;
        BestHangCount = bestHangingWeek.HangCount;

        var worstHangingWeek = weeks.Last();
        WorstHangWeekNumber = worstHangingWeek.Number;
        WorstHangCount = worstHangingWeek.HangCount;

        var bestBusRideWeek = weeks.OrderByDescending(w => w.BusRides).First();
        BestBusRideWeekNumber = bestBusRideWeek.Number;
        BestBusRideCount = bestBusRideWeek.BusRides;
    }
}
