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
    IRecipient<PersonAddedOrChangedMessage>,
    IRecipient<PersonDeletedMessage>,
    IRecipient<BowlerHangCountChangedMessage>,
    IRecipient<SystemResetMessage>,
    IRecipient<BowlerSubChangedMessage>
{
    private readonly IAudioService _audioService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly ISettingsService _settingsService;
    private readonly IWeekService _weekService;
    private readonly IBowlerService _bowlerService;

    private readonly IMapper<CurrentWeekListItemViewModel, Bowler> _bowlerMapper;
    private readonly IMapper<IEnumerable<Bowler>, IEnumerable<CurrentWeekListItemViewModel>> _currentWeekListItemViewModelMapper;

    public CurrentWeekOverviewViewModel(
        IAudioService audioService,
        IDialogService dialogService,
        INavigationService navigationService,
        ISettingsService settingsService,
        IWeekService weekService,
        IBowlerService bowlerService,
        IMapper<CurrentWeekListItemViewModel, Bowler> bowlerMapper,
        IMapper<IEnumerable<Bowler>, IEnumerable<CurrentWeekListItemViewModel>> currentWeekListItemViewModelMapper)
    {
        _audioService = audioService;
        _dialogService = dialogService;
        _navigationService = navigationService;
        _settingsService = settingsService;
        _weekService = weekService;
        _bowlerService = bowlerService;

        _bowlerMapper = bowlerMapper;
        _currentWeekListItemViewModelMapper = currentWeekListItemViewModelMapper;

        WeakReferenceMessenger.Default.Register<PersonAddedOrChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<PersonDeletedMessage>(this);
        WeakReferenceMessenger.Default.Register<BowlerHangCountChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<SystemResetMessage>(this);
        WeakReferenceMessenger.Default.Register<BowlerSubChangedMessage>(this);
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
                await _audioService.PlaySoundAsync(Constants.Files.BusRideSoundFileName);
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
        if (CurrentWeekBowlers.Count == 0)
        {
            await Loading(GetCurrentWeek);
        }

        InitializeCurrentWeekPageSettings();
    }

    [RelayCommand]
    private async Task SetBowlerStatusToActive(CurrentWeekListItemViewModel? vm) => await SetBowlerStatus(vm, Enums.Status.Active);

    [RelayCommand]
    private async Task SetBowlerStatusToBlind(CurrentWeekListItemViewModel? vm) => await SetBowlerStatus(vm, Enums.Status.Blind);

    [RelayCommand]
    private async Task SetBowlerStatusToUsingSub(CurrentWeekListItemViewModel? vm) => await SetBowlerStatus(vm, Enums.Status.UsingSub);

    [RelayCommand]
    private async Task SubmitWeek()
    {
        if (!await _dialogService.Ask("Complete Week", $"Are you ready to complete week {CurrentWeek.Number}?", "Yes", "No"))
        {
            return;
        }

        await _weekService.CreateWeek(CurrentWeek.Number + 1).ContinueWith(async saveTask =>
        {
            if (saveTask.IsCompletedSuccessfully)
            {
                var newWeek = await saveTask;
                _settingsService.CurrentWeekPrimaryKey = newWeek.Id;
                await GetCurrentWeek();
                InitializeCurrentWeekPageSettings();
            }
            else
            {
                await _dialogService.AlertAsync("Error", "Unable to create new week.", "Ok");
            }
        });
    }

    private async Task GetCurrentWeek()
    {
        CurrentWeek = await _weekService.GetWeekById(_settingsService.CurrentWeekPrimaryKey);
        if (CurrentWeek is not null)
        {
            _settingsService.CurrentWeekPrimaryKey = CurrentWeek.Id;
            if (CurrentWeek.Bowlers.Count > 0)
            {
                CurrentWeekBowlers.Clear();
                CurrentWeekBowlers = _currentWeekListItemViewModelMapper.Map(CurrentWeek.Bowlers).ToObservableCollection();
                CurrentWeekBowlers.SetLowestBowlerHangCount();
            }

            MapWeekData(CurrentWeek);
        }
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

    private async Task SetBowlerStatus(CurrentWeekListItemViewModel? vm, Enums.Status status)
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
                await _bowlerService.UpdateBowler(_bowlerMapper.Map(vm));
                break;
            case Enums.Status.Blind:
                vm.SubId = null;
                vm.Status = Enums.Status.Blind;
                await _bowlerService.UpdateBowler(_bowlerMapper.Map(vm));
                break;
            case Enums.Status.UsingSub:
                await _navigationService.GoToSelectSub(_bowlerMapper.Map(vm));
                return;
        }

        await GetCurrentWeek();
    }

    public async void Receive(SystemResetMessage message)
    {
        CurrentWeekBowlers.Clear();
        TeamHangTotal = 0;
        IsEnableCompleteWeek = false;
        PageTitle = "Week 1";
        await GetCurrentWeek();
    }

    public async void Receive(BowlerSubChangedMessage message)
    {
        var bowler = CurrentWeekBowlers.FirstOrDefault(b => b.BowlerId == message.Id);
        if (bowler is null)
        {
            return;
        }

        bowler.SubId = message.SubId;
        bowler.Status = Enums.Status.UsingSub;
        await _bowlerService.UpdateBowler(_bowlerMapper.Map(bowler));

        await GetCurrentWeek();
    }

    public async void Receive(BowlerHangCountChangedMessage message)
    {
        var bowler = CurrentWeekBowlers.FirstOrDefault(b => b.BowlerId == message.BowlerId);
        if (bowler is null)
        {
            return;
        }

        if (await _bowlerService.UpdateBowler(_bowlerMapper.Map(bowler)))
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
        }
        else
        {
            await _dialogService.AlertAsync("Error", "Unable to update bowler hang count", "Ok");
        }
    }

    public async void Receive(PersonAddedOrChangedMessage message)
    {
        if (message.Id > 0 && !message.IsSub && !CurrentWeekBowlers.Any(b => b.PersonId == message.Id))
        {
            var bowler = new Bowler
            {
                PersonId = message.Id,
                WeekId = _settingsService.CurrentWeekPrimaryKey,
            };
            await _bowlerService.AddBowler(bowler);
        }

        CurrentWeekBowlers.Clear();
        await GetCurrentWeek();
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
