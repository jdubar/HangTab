using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    private readonly IAudioService _audioService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly ISettingsService _settingsService;
    private readonly IWeekService _weekService;
    private readonly IWeeklyLineupService _weeklyLineupService;

    public CurrentWeekOverviewViewModel(
        IAudioService audioService,
        IDialogService dialogService,
        INavigationService navigationService,
        ISettingsService settingsService,
        IWeekService weekService,
        IWeeklyLineupService weeklyLineupService)
    {
        _audioService = audioService;
        _dialogService = dialogService;
        _navigationService = navigationService;
        _settingsService = settingsService;
        _weekService = weekService;
        _weeklyLineupService = weeklyLineupService;

        WeakReferenceMessenger.Default.Register<BowlerAddedOrChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<BowlerDeletedMessage>(this);
        WeakReferenceMessenger.Default.Register<BowlerHangCountChangedMessage>(this);
    }

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _weekNumber;

    [ObservableProperty]
    private int _busRides;

    async partial void OnBusRidesChanged(int value)
    {
        if (value >= 0)
        {
            if (CurrentWeek.BusRides < value)
            {
                PlayBusRideAnimation = true;
                _audioService.PlayBusRideSound();
                await Task.Delay(3000);
                PlayBusRideAnimation = false;
            }

            CurrentWeek.BusRides = value;
            await _weekService.UpdateWeek(CurrentWeek);
        }
    }

    private Week CurrentWeek { get; set; } = new Week();

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private int _teamHangTotal;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BusRideStepperEnabled))]
    private ObservableCollection<CurrentWeekListItemViewModel> _currentWeekBowlers = [];

    public bool BusRideStepperEnabled => CurrentWeekBowlers.Count > 0;

    [ObservableProperty]
    private CurrentWeekListItemViewModel? _selectedBowler = default!;

    [ObservableProperty]
    private bool _isEnableCompleteWeek = true;

    [ObservableProperty]
    private bool _playPopperAnimation;

    [ObservableProperty]
    private bool _playBusRideAnimation;

    // TODO: Add Complete Week (submit) relay command

    [RelayCommand]
    private async Task NavigateToSwitchSelectedBowler()
    {
        if (SelectedBowler is not null)
        {
            await _navigationService.GoToSwitchBowler(SelectedBowler.MapCurrentWeekListItemViewModelToWeeklyLineup());
            SelectedBowler = null;
        }
    }

    public override async Task LoadAsync()
    {
        if (CurrentWeekBowlers.Count == 0)
        {
            await Loading(GetCurrentWeek);
        }

        IsEnableCompleteWeek = CurrentWeekBowlers.Count > 0;
        TeamHangTotal = CurrentWeekBowlers.Sum(b => b.HangCount);
        PageTitle = $"Week {WeekNumber} of {_settingsService.TotalSeasonWeeks}";
    }

    private async Task GetCurrentWeek()
    {
        CurrentWeek = _settingsService.CurrentWeekPrimaryKey == 0
            ? await CreateFirstWeek()
            : await _weekService.GetWeek(_settingsService.CurrentWeekPrimaryKey);

        if (CurrentWeek is not null)
        {
            if (CurrentWeek.Bowlers.Count > 0)
            {
                CurrentWeekBowlers.Clear();
                CurrentWeekBowlers = CurrentWeek.Bowlers.Where(b => !b.Bowler.IsInactive).MapBowlerToBowlerListItemViewModel().ToObservableCollection();
            }

            MapWeekData(CurrentWeek);
        }
    }

    private async Task<Week> CreateFirstWeek()
    {
        var week = await _weekService.CreateWeek(1);
        _settingsService.CurrentWeekPrimaryKey = week.Id;
        return week;
    }

    public async void Receive(BowlerHangCountChangedMessage message)
    {
        var bowler = CurrentWeekBowlers.FirstOrDefault(b => b.BowlerId == message.Id);
        if (bowler is null)
        {
            return;
        }

        if (await _weeklyLineupService.UpdateWeeklyLineup(bowler.MapCurrentWeekListItemViewModelToWeeklyLineup()))
        {
            var newHangTotal = CurrentWeekBowlers.Sum(b => b.HangCount);
            var isIncrease = newHangTotal > TeamHangTotal;
            TeamHangTotal = CurrentWeekBowlers.Sum(b => b.HangCount);

            if (isIncrease)
            {
                PlayPopperAnimation = true;
                await Task.Delay(1000);
                PlayPopperAnimation = false;
            }
        }
        else
        {
            await _dialogService.AlertAsync("Error", "Unable to update bowler hang count", "Ok");
        }
    }

    public async void Receive(BowlerAddedOrChangedMessage message)
    {
        if (message.Id > 0 && !message.IsSub)
        {
            var weeklyLineup = new WeeklyLineup
            {
                BowlerId = message.Id,
                WeekId = _settingsService.CurrentWeekPrimaryKey,
            };
            await _weeklyLineupService.AddWeeklyLineupBowler(weeklyLineup);
            CurrentWeekBowlers.Add(weeklyLineup.MapWeeklyLineupToCurrentWeekListItemViewModel());
        }

        CurrentWeekBowlers.Clear();
        await GetCurrentWeek();
    }

    public void Receive(BowlerDeletedMessage message)
    {
        var deletedBowler = CurrentWeekBowlers.FirstOrDefault(b => b.BowlerId == message.Id);
        if (deletedBowler is not null)
        {
            CurrentWeekBowlers.Remove(deletedBowler);
        }
    }

    private void MapWeekData(Week week)
    {
        Id = week.Id;
        WeekNumber = week.WeekNumber;
        BusRides = week.BusRides;
    }
}
