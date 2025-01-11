using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Messages;
using HangTab.Models;
using HangTab.Services;
using HangTab.Services.Mappers;
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
    private Week _week;

    [ObservableProperty]
    private ObservableCollection<BowlerListItemViewModel> _bowlers = [];

    public override async Task LoadAsync()
    {
        PageTitle = $"Week {_settingsService.CurrentSeasonWeek} of {_settingsService.TotalSeasonWeeks}";

        if (Bowlers.Count == 0)
        {
            await Loading(GetCurrentWeek);
        }
    }

    private async Task GetCurrentWeek()
    {
        Week = await _weekService.GetWeekByWeekNumber(_settingsService.CurrentSeasonWeek);

        Bowlers.Clear();
        Bowlers = Week.Bowlers.Map().ToObservableCollection();
        //var bowlers = await _bowlerService.GetBowlers();
        //var week = await _weekService.GetWeekByWeekNumber(_settingsService.CurrentSeasonWeek);
        //if (week is null)
        //{
        //    var newWeek = new Week
        //        {
        //            WeekNumber = _settingsService.CurrentSeasonWeek,
        //            BusRides = 0,
        //            Bowlers = bowlers
        //        };

        //    if (await _weekService.CreateWeek(newWeek))
        //    {
        //        week = newWeek;
        //    }
        //    else
        //    {
        //        return; // TODO: Add error handling
        //    }
        //}

        //var listItems = week.Bowlers.Join(
        //    await _bowlerService.GetBowlers(),
        //    w => w.BowlerId,
        //    b => b.Id,
        //    (w, b) => new BowlerListItemViewModel(
        //        b.Id,
        //        b.Name,
        //        b.IsSub,
        //        w.HangCount,
        //        w.Position,
        //        b.ImageUrl,
        //        w.Status));

        //Bowlers.Clear();
        //Bowlers = listItems.ToObservableCollection();
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
