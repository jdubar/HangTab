using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Messages;
using HangTab.Services;
using HangTab.ViewModels.Base;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class CurrentWeekOverviewViewModel :
    ViewModelBase,
    IRecipient<BowlerAddedOrChangedMessage>,
    IRecipient<BowlerDeletedMessage>
{
    private readonly IBowlerService _bowlerService;
    private readonly IDatabaseService _databaseService;
    private readonly ISettingsService _settingsService;
    private readonly IWeekService _weekService;

    public CurrentWeekOverviewViewModel(
        IBowlerService bowlerService,
        IDatabaseService databaseService,
        ISettingsService settingsService,
        IWeekService weekService)
    {
        _bowlerService = bowlerService;
        _databaseService = databaseService;
        _settingsService = settingsService;
        _weekService = weekService;

        WeakReferenceMessenger.Default.Register<BowlerAddedOrChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<BowlerDeletedMessage>(this);
    }

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private ObservableCollection<BowlerListItemViewModel> _bowlers = [];

    public override async Task LoadAsync()
    {
        PageTitle = $"Week {_settingsService.CurrentSeasonWeek} of {_settingsService.TotalSeasonWeeks}";

        if (Bowlers.Count == 0)
        {
            await Loading(GetBowlers);
        }
    }

    private async Task GetBowlers()
    {
        var week = await _weekService.GetWeekByWeekNumber(_settingsService.CurrentSeasonWeek);
        var bowlers = week.Bowlers.Join(
             await _bowlerService.GetBowlers(),
            w => w.BowlerId,
            b => b.Id,
            (w, b) => new BowlerListItemViewModel(
                b.Id,
                b.Name,
                b.IsSub,
                w.HangCount,
                w.Position,
                b.ImageUrl,
                w.Status));

        List<BowlerListItemViewModel> listItems = [];
        foreach (var bowler in bowlers)
        {
            listItems.Add(bowler);
        }

        Bowlers.Clear();
        Bowlers = listItems.ToObservableCollection();
    }

    public async void Receive(BowlerAddedOrChangedMessage message)
    {
        // TODO: Add logic
    }

    public void Receive(BowlerDeletedMessage message)
    {
        // TODO: Add logic
    }
}
