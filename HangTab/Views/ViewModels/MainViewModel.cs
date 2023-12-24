using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels;

using System.Collections.ObjectModel;

namespace HangTab.Views.ViewModels;
public partial class MainViewModel(IDatabaseService dbservice) : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<BowlerViewModel> _mainBowlers;

    [ObservableProperty]
    private ObservableCollection<BowlerViewModel> _switchBowlers;

    [ObservableProperty]
    private BowlerViewModel _workingBowlerViewModel;

    [ObservableProperty]
    private BowlerViewModel _selectedBowler;

    [ObservableProperty]
    private int _busRidesLabel;

    [ObservableProperty]
    private int _totalBusRidesLabel;

    private BusRideViewModel BusRideViewModel { get; set; }

    private int WorkingWeek { get; set; }

    [RelayCommand]
    private async Task BusRideAsync()
    {
        await ExecuteAsync(async () =>
        {
            BusRideViewModel.BusRide.TotalBusRides++;
            BusRideViewModel.BusRideWeek.BusRides++;

            await dbservice.UpdateBusRidesByWeek(BusRideViewModel, WorkingWeek);

            SetBusRideLabels();
        }, "Bus Ride!!!");
    }

    [RelayCommand]
    private async Task HangBowlerAsync(BowlerViewModel viewModel)
    {
        await ExecuteAsync(async () =>
        {
            viewModel.Bowler.TotalHangings++;
            viewModel.BowlerWeek.Hangings++;
            viewModel.BowlerWeek.WeekNumber = WorkingWeek;

            await dbservice.UpdateBowlerHangingsByWeek(viewModel, WorkingWeek);
        }, "Hanging bowler...");
    }

    public async Task LoadMainBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            if (WorkingWeek == 0)
            {
                WorkingWeek = await dbservice.SetWorkingWeek();
            }

            await LoadBusRidesAsync();

            SetBusRideLabels();

            SetWorkingBowlerViewModelCommand.Execute(new());
            await SetMainBowlersListAsync();
        }, "Loading bowlers...");
    }

    public async Task LoadSwitchBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            await SetSwitchBowlersListAsync();
        }, "Loading bowlers...");
    }

    [RelayCommand]
    private void SetWorkingBowlerViewModel(BowlerViewModel viewModel) =>
        WorkingBowlerViewModel = viewModel ?? new();

    [RelayCommand]
    private async Task ShowSwitchBowlerViewAsync(BowlerViewModel bowler)
    {
        SetWorkingBowlerViewModelCommand.Execute(bowler);
        await Shell.Current.GoToAsync(nameof(SwitchBowlerPage), true);
    }

    [RelayCommand]
    private async Task StartNewWeekAsync()
    {
        await ExecuteAsync(async () =>
        {
            WorkingWeek++;
            await LoadMainBowlersAsync();
        }, "Starting new week...");
    }

    [RelayCommand]
    private async Task SwitchBowlerAsync()
    {
        await ExecuteAsync(async () =>
        {
            await ChangeBowlerHiddenStateAsync(WorkingBowlerViewModel);
            await ChangeBowlerHiddenStateAsync(SelectedBowler);

            var index = MainBowlers.IndexOf(WorkingBowlerViewModel);
            MainBowlers.RemoveAt(index);
            MainBowlers.Insert(index, SelectedBowler);
        }, "Switching bowler...");

        await Shell.Current.GoToAsync("..", true);
    }

    private async Task ChangeBowlerHiddenStateAsync(BowlerViewModel viewModel)
    {
        viewModel.Bowler.IsHidden = !viewModel.Bowler.IsHidden;
        await dbservice.UpdateBowler(viewModel.Bowler);
    }

    private ObservableCollection<BowlerViewModel> LoadBowlers(IEnumerable<Bowler> bowlers, IEnumerable<BowlerWeek> weeks)
    {
        var collection = new ObservableCollection<BowlerViewModel>();

        foreach (var bowler in bowlers)
        {
            var week = weeks.FirstOrDefault(w => w.BowlerId == bowler.Id);
            week ??= new BowlerWeek()
            {
                WeekNumber = WorkingWeek,
                BowlerId = bowler.Id,
                Hangings = 0
            };

            var viewModel = new BowlerViewModel()
            {
                Bowler = bowler,
                BowlerWeek = week
            };
            collection.Add(viewModel);
        }
        return collection;
    }

    private async Task LoadBusRidesAsync()
    {
        await ExecuteAsync(async () =>
        {
            BusRideViewModel = await dbservice.GetLatestBusRideWeek(WorkingWeek);
        });
    }

    private void SetBusRideLabels()
    {
        BusRidesLabel = BusRideViewModel.BusRideWeek.BusRides;
        TotalBusRidesLabel = BusRideViewModel.BusRide.TotalBusRides;
    }

    private async Task SetMainBowlersListAsync()
    {
        MainBowlers ??= [];
        var bowlers = await dbservice.GetMainBowlers();
        var weeks = await dbservice.GetWeeksByWeek(WorkingWeek);
        if (bowlers is not null && bowlers.Any())
        {
            MainBowlers.Clear();
            MainBowlers = LoadBowlers(bowlers, weeks);
        }
    }

    private async Task SetSwitchBowlersListAsync()
    {
        SwitchBowlers ??= [];
        var bowlers = await dbservice.GetSwitchBowlers(WorkingBowlerViewModel.Bowler.Id);
        var weeks = await dbservice.GetAllWeeks();
        if (bowlers is not null && bowlers.Any())
        {
            SwitchBowlers.Clear();
            SwitchBowlers = LoadBowlers(bowlers, weeks);
        }
    }
}
