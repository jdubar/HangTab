using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
    INavigationService navigationService,
    IPersonService personService,
    IWeekService weekService,
    IMapper<IEnumerable<Person>, IEnumerable<BowlerListItemViewModel>> mapper,
    DataManagerViewModel dataManager) : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<BowlerListItemViewModel> _bestBowlers = [];

    [ObservableProperty]
    private int _bestHangWeekNumber;

    [ObservableProperty]
    private int _bestHangWeekCount;

    [ObservableProperty]
    private int _bestBusRideWeekNumber;

    [ObservableProperty]
    private int _bestBusRideWeekCount;

    [ObservableProperty]
    private int _worstHangWeekNumber;

    [ObservableProperty]
    private int _worstHangWeekCount;

    public override async Task LoadAsync()
    {
        if (BestBowlers.Count == 0)
        {
            await Loading(async () =>
            {
                await GetBowlers();
                await GetWeeks();
            });
        }
    }

    [RelayCommand]
    private async Task SubmitSeasonAsync()
    {
        await dataManager.StartNewSeasonAsync();
        await navigationService.GoToHome();
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

        BestBowlers = bowlers.Where(b => b.HangCount == bowlers.Min(b => b.HangCount)).ToObservableCollection();
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
        BestHangWeekCount = bestHangingWeek.HangCount;

        var worstHangingWeek = weeks.Last();
        WorstHangWeekNumber = worstHangingWeek.Number;
        WorstHangWeekCount = worstHangingWeek.HangCount;

        var bestBusRideWeek = weeks.OrderByDescending(w => w.BusRides).First();
        BestBusRideWeekNumber = bestBusRideWeek.Number;
        BestBusRideWeekCount = bestBusRideWeek.BusRides;
    }
}
