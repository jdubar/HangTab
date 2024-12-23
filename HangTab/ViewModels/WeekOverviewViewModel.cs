using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.ViewModels.Base;

using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;

namespace HangTab.ViewModels;
public partial class WeekOverviewViewModel : ViewModelBase, IRecipient<WeeklyLineupChangedMessage>
{
    private readonly ISettingsService _settingsService;
    private readonly IWeekService _weekService;

    public WeekOverviewViewModel(
        ISettingsService settingsService,
        IWeekService weekService)
    {
        _settingsService = settingsService;
        _weekService = weekService;

        WeakReferenceMessenger.Default.Register(this);
    }

    [ObservableProperty]
    private ObservableCollection<WeeklyLineupViewModel> _bowlers = [];

    public override async Task LoadAsync()
    {
        if (Bowlers.Count == 0)
        {
            await Loading(GetCurrentWeekBowlers);
        }
    }

    private async Task GetCurrentWeekBowlers()
    {
        var week = await _weekService.GetWeek(_settingsService.CurrentSeasonWeek);
        var bowlers = week.Bowlers;
        List<WeeklyLineupViewModel> listItems = [];
        foreach (var bowler in bowlers)
        {
            listItems.Add(MapWeeklyLineupToWeeklyLineupViewModel(bowler));
        }

        Bowlers.Clear();
        Bowlers = listItems.ToObservableCollection();
    }

    private WeeklyLineupViewModel MapWeeklyLineupToWeeklyLineupViewModel(WeeklyLineup weeklyLineup)
    {
        var bowlerViewModel = new BowlerViewModel
        {
            Id = weeklyLineup.BowlerId,
            FirstName =weeklyLineup.Bowler.FirstName,
            LastName = weeklyLineup.Bowler.LastName ?? string.Empty,
            ImageUrl = weeklyLineup.Bowler.ImageUrl,
            IsSub = weeklyLineup.Bowler.IsSub
        };

        return new WeeklyLineupViewModel(
            weeklyLineup.Id,
            weeklyLineup.Position,
            weeklyLineup.Status,
            weeklyLineup.HangCount,
            bowlerViewModel);
    }

    public async void Receive(WeeklyLineupChangedMessage message)
    {
        try
        {
            Bowlers.Clear();
            await GetCurrentWeekBowlers();
        }
        catch (Exception)
        {
            // TODO: Handle exception
        }
    }
}
