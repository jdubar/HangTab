using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Extensions;
using HangTab.Mappers;
using HangTab.Messages;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels.Base;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class CurrentWeekOverviewViewModel :
    ViewModelBase,
    IRecipient<BowlerSubChangedMessage>,
    IRecipient<PersonAddedOrChangedMessage>,
    IRecipient<PersonDeletedMessage>,
    IRecipient<SystemResetMessage>
{
    private readonly IAudioService _audioService;
    private readonly IBowlerService _bowlerService;
    private readonly IDialogService _dialogService;
    private readonly IMessenger _messenger;
    private readonly INavigationService _navigationService;
    private readonly ISettingsService _settingsService;
    private readonly IScreenshotService _screenshotService;
    private readonly IShareService _shareService;
    private readonly IWeekService _weekService;

    private const string BusSound = "beepbeep.mp3";

    public CurrentWeekOverviewViewModel(
        IAudioService audioService,
        IBowlerService bowlerService,
        IDialogService dialogService,
        IMessenger messenger,
        INavigationService navigationService,
        IScreenshotService screenshotService,
        ISettingsService settingsService,
        IShareService shareService,
        IWeekService weekService)
    {
        _audioService = audioService;
        _bowlerService = bowlerService;
        _dialogService = dialogService;
        _messenger = messenger;
        _navigationService = navigationService;
        _screenshotService = screenshotService;
        _settingsService = settingsService;
        _shareService = shareService;
        _weekService = weekService;

        _messenger.Register<BowlerSubChangedMessage>(this);
        _messenger.Register<PersonAddedOrChangedMessage>(this);
        _messenger.Register<PersonDeletedMessage>(this);
        _messenger.Register<SystemResetMessage>(this);
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
                await _audioService.PlaySoundAsync(BusSound);
                await Task.Delay(1500);
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

    public override async Task LoadAsync()
    {
        if (_settingsService.SeasonComplete)
        {
            await _navigationService.GoToSeasonSummary();
            return;
        }

        if (CurrentWeekBowlers.Count == 0)
        {
            await Loading(GetCurrentWeekAsync);
        }

        InitializeCurrentWeekPageSettings();
    }

    [RelayCommand]
    private async Task SetBowlerStatusToActiveAsync(CurrentWeekListItemViewModel? vm) => await SetBowlerStatusAsync(vm, Enums.Status.Active);

    [RelayCommand]
    private async Task SetBowlerStatusToBlindAsync(CurrentWeekListItemViewModel? vm) => await SetBowlerStatusAsync(vm, Enums.Status.Blind);

    [RelayCommand]
    private async Task SetBowlerStatusToUsingSubAsync(CurrentWeekListItemViewModel? vm) => await SetBowlerStatusAsync(vm, Enums.Status.UsingSub);

    [RelayCommand]
    private async Task ShareScreenshotAsync()
    {
        var screenshot = await _screenshotService.TakeScreenshotAsync();
        if (string.IsNullOrWhiteSpace(screenshot))
        {
            await _dialogService.AlertAsync("Error", "Unable to take screenshot.", "Ok");
        }
        else
        {
            await _shareService.ShareFileAsync(screenshot);
        }
    }

    [RelayCommand]
    private async Task SubmitWeekAsync()
    {
        if (!await _dialogService.Ask("Complete Week", $"Are you ready to complete week {CurrentWeek.Number}?", "Yes", "No"))
        {
            return;
        }

        if (CurrentWeek.Number < _settingsService.TotalSeasonWeeks)
        {
            await Loading(StartNewWeekAsync);
        }
        else
        {
            _settingsService.SeasonComplete = true;
            await _navigationService.GoToSeasonSummary();
        }
    }

    private async Task StartNewWeekAsync()
    {
        await _weekService.CreateWeek(CurrentWeek.Number + 1).ContinueWith(async saveTask =>
        {
            if (saveTask.IsCompletedSuccessfully)
            {
                var newWeek = await saveTask;
                _settingsService.CurrentWeekPrimaryKey = newWeek.Id;
                await GetCurrentWeekAsync();
                InitializeCurrentWeekPageSettings();
            }
            else
            {
                await _dialogService.AlertAsync("Error", "Unable to create new week.", "Ok");
            }
        });
    }

    private async Task GetCurrentWeekAsync()
    {
        CurrentWeek = await _weekService.GetWeekById(_settingsService.CurrentWeekPrimaryKey);
        if (CurrentWeek is null)
        {
            return;
        }

        _settingsService.CurrentWeekPrimaryKey = CurrentWeek.Id;
        if (CurrentWeek.Bowlers.Count > 0)
        {
            CurrentWeekBowlers.Clear();
            CurrentWeekBowlers = CurrentWeek.Bowlers.ToCurrentWeekListItemViewModelList().ToObservableCollection();
            CurrentWeekBowlers.SetLowestBowlerHangCount();
        }

        MapWeekData(CurrentWeek);
    }

    private void InitializeCurrentWeekPageSettings()
    {
        IsEnableCompleteWeek = CurrentWeekBowlers.Count > 0;
        TeamHangTotal = CurrentWeekBowlers.Sum(b => b.HangCount);
        PageTitle = $"Week {WeekNumber} of {_settingsService.TotalSeasonWeeks}";
    }

    private void MapWeekData(Week week)
    {
        Id = week.Id;
        WeekNumber = week.Number;
        BusRides = week.BusRides;
    }

    private async Task SetBowlerStatusAsync(CurrentWeekListItemViewModel? vm, Enums.Status status)
    {
        if (vm is null)
        {
            return;
        }

        switch (status)
        {
            case Enums.Status.Active:
                vm.SubId = null;
                vm.Status = Enums.Status.Active;
                await _bowlerService.UpdateBowler(vm.ToBowler());
                break;
            case Enums.Status.Blind:
                vm.SubId = null;
                vm.Status = Enums.Status.Blind;
                await _bowlerService.UpdateBowler(vm.ToBowler());
                break;
            case Enums.Status.UsingSub:
                await _navigationService.GoToSelectSub(vm.ToBowler());
                return;
        }

        await GetCurrentWeekAsync();
    }

    [RelayCommand]
    private async Task BowlerHangCountChangedAsync(CurrentWeekListItemViewModel? vm)
    {
        if (vm is null)
        {
            return;
        }

        if (await _bowlerService.UpdateBowler(vm.ToBowler()))
        {
            var newHangTotal = CurrentWeekBowlers.Sum(b => b.HangCount);
            var isIncrease = newHangTotal > TeamHangTotal;
            TeamHangTotal = CurrentWeekBowlers.Sum(b => b.HangCount);
            CurrentWeekBowlers.SetLowestBowlerHangCount();

            if (isIncrease)
            {
                PlayPopperAnimation = true;
                await Task.Delay(1000);
                PlayPopperAnimation = false;
            }

            _messenger.Send(new BowlerHangCountChangedMessage(vm.BowlerId, vm.HangCount));
        }
        else
        {
            await _dialogService.AlertAsync("Error", "Unable to update bowler hang count", "Ok");
        }
    }

    public async void Receive(SystemResetMessage message)
    {
        CurrentWeekBowlers.Clear();
        TeamHangTotal = 0;
        IsEnableCompleteWeek = false;
        PageTitle = "Week 1";
        await GetCurrentWeekAsync();
    }

    public async void Receive(BowlerSubChangedMessage message)
    {
        var vm = CurrentWeekBowlers.FirstOrDefault(b => b.BowlerId == message.Id);
        if (vm is null)
        {
            return;
        }

        vm.SubId = message.SubId;
        vm.Status = Enums.Status.UsingSub;
        await _bowlerService.UpdateBowler(vm.ToBowler());

        await GetCurrentWeekAsync();
    }

    public async void Receive(PersonAddedOrChangedMessage message)
    {
        if (message.Id > 0)
        {
            var bowler = new Bowler
            {
                PersonId = message.Id,
                WeekId = _settingsService.CurrentWeekPrimaryKey,
            };

            if (!message.IsSub && !CurrentWeekBowlers.Any(b => b.PersonId == message.Id))
            {
                await _bowlerService.AddBowler(bowler);
            }
            else if (message.IsSub && CurrentWeekBowlers.Any(b => b.PersonId == message.Id))
            {
                await _bowlerService.RemoveBowler(message.Id);
            }
        }

        CurrentWeekBowlers.Clear();
        await GetCurrentWeekAsync();
    }

    public void Receive(PersonDeletedMessage message)
    {
        var deletedBowler = CurrentWeekBowlers.FirstOrDefault(b => b.BowlerId == message.Id);
        if (deletedBowler is not null)
        {
            CurrentWeekBowlers.Remove(deletedBowler);
            CurrentWeekBowlers.SetLowestBowlerHangCount();
        }
    }
}
