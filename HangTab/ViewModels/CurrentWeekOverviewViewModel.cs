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
    IRecipient<BowlerDeletedMessage>,
    IRecipient<BowlerHangCountChangedMessage>
{
    private readonly IBowlerService _bowlerService;
    private readonly IDialogService _dialogService;
    private readonly ISettingsService _settingsService;
    private readonly IWeekService _weekService;
    private readonly IWeeklyLineupService _weeklyLineupService;

    public CurrentWeekOverviewViewModel(
        IBowlerService bowlerService,
        IDialogService dialogService,
        ISettingsService settingsService,
        IWeekService weekService,
        IWeeklyLineupService weeklyLineupService)
    {
        _bowlerService = bowlerService;
        _dialogService = dialogService;
        _settingsService = settingsService;
        _weekService = weekService;
        _weeklyLineupService = weeklyLineupService;

        WeakReferenceMessenger.Default.Register<BowlerAddedOrChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<BowlerDeletedMessage>(this);
        WeakReferenceMessenger.Default.Register<BowlerHangCountChangedMessage>(this);
    }

    [ObservableProperty]
    private string _pageTitle;

    [ObservableProperty]
    private int _teamHangTotal;

    [ObservableProperty]
    private Week _week = new();

    [ObservableProperty]
    private ObservableCollection<BowlerListItemViewModel> _currentWeekBowlers = [];

    [ObservableProperty]
    private BowlerListItemViewModel _selectedBowler;

    public override async Task LoadAsync()
    {
        if (CurrentWeekBowlers.Count == 0)
        {
            await Loading(GetCurrentWeek);
        }

        TeamHangTotal = Week.Bowlers.Sum(b => b.HangCount);
        PageTitle = $"Week {Week.WeekNumber} of {_settingsService.TotalSeasonWeeks}";
    }

    private async Task GetCurrentWeek()
    {
        if (_settingsService.CurrentWeekPrimaryKey == 0)
        {
            Week = await _weekService.CreateWeek(1);
            _settingsService.CurrentWeekPrimaryKey = Week.Id;
        }
        else
        {
            CurrentWeekBowlers.Clear();
            Week = await _weekService.GetWeek(_settingsService.CurrentWeekPrimaryKey);
            if (Week.Bowlers.Count > 0)
            {
                foreach (var lineup in Week.Bowlers)
                {
                    lineup.Bowler = await _bowlerService.GetBowlerById(lineup.BowlerId);
                }
                CurrentWeekBowlers = Week.Bowlers.MapWeeklyLineupToBowlerListItemViewModel().ToObservableCollection();
            }
        }
    }

    public async void Receive(BowlerHangCountChangedMessage message)
    {
        var bowler = Week.Bowlers.FirstOrDefault(wl => wl.BowlerId == message.Id);
        if (bowler is null)
        {
            return;
        }

        bowler.HangCount = message.HangCount;
        TeamHangTotal = Week.Bowlers.Sum(b => b.HangCount);

        if (!await _weeklyLineupService.UpdateWeeklyLineup(bowler))
        {
            await _dialogService.Notify("Error", "Unable to update bowler hang count");
        }
    }

    public async void Receive(BowlerAddedOrChangedMessage message)
    {
        if (message.Id > 0)
        {
            var weeklyLineup = new WeeklyLineup
            {
                BowlerId = message.Id,
                WeekId = Week.Id,
            };
            await _weeklyLineupService.AddWeeklyLineupBowler(weeklyLineup);
            Week.Bowlers.Add(weeklyLineup);
            await _weekService.UpdateWeek(Week);
        }

        CurrentWeekBowlers.Clear();
        await GetCurrentWeek();
    }

    public void Receive(BowlerDeletedMessage message)
    {
        var deletedBowler = CurrentWeekBowlers.FirstOrDefault(b => b.Id == message.Id);
        if (deletedBowler is not null)
        {
            CurrentWeekBowlers.Remove(deletedBowler);
        }
    }
}
