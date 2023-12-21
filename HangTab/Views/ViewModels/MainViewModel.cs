using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Data;
using HangTab.Models;
using HangTab.ViewModels;

using System.Collections.ObjectModel;

namespace HangTab.Views.ViewModels;
public partial class MainViewModel(DatabaseContext context) : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<BowlerViewModel> _mainBowlers;

    [ObservableProperty]
    private ObservableCollection<BowlerViewModel> _switchBowlers;

    [ObservableProperty]
    private BowlerViewModel _workingBowlerViewModel;

    [ObservableProperty]
    private BusRideViewModel _workingBusRideViewModel;

    [ObservableProperty]
    private BowlerViewModel _selectedBowler;

    [ObservableProperty]
    private int _busRides;

    [ObservableProperty]
    private int _totalBusRides;

    private int WorkingWeek { get; set; }


    [RelayCommand]
    private async Task BusRideAsync()
    {
        await ExecuteAsync(async () =>
        {
            WorkingBusRideViewModel.BusRide.TotalBusRides++;
            WorkingBusRideViewModel.BusRideWeek.BusRides++;

            _ = await context.UpdateItemAsync(WorkingBusRideViewModel.BusRide);
            var busRides = await context.GetFilteredAsync<BusRideWeek>(b => b.WeekNumber == WorkingWeek);
            _ = busRides is not null && busRides.Any()
                ? await context.UpdateItemAsync(WorkingBusRideViewModel.BusRideWeek)
                : await context.AddItemAsync(WorkingBusRideViewModel.BusRideWeek);

            SetBusRideCounts();
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

            _ = await context.UpdateItemAsync(viewModel.Bowler);

            var weeks = await context.GetFilteredAsync<BowlerWeek>(w => w.WeekNumber == WorkingWeek
                                                                        && w.BowlerId == viewModel.Bowler.Id);
            _ = weeks is not null && weeks.Any()
                ? await context.UpdateItemAsync(viewModel.BowlerWeek)
                : await context.AddItemAsync(viewModel.BowlerWeek);
        }, "Hanging bowler...");
    }

    [RelayCommand]
    public async Task LoadMainBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            await SetWorkingWeekAsync(context);

            await LoadBusRidesAsync();
            SetBusRideCounts();

            SetWorkingBowlerViewModelCommand.Execute(new());
            await SetMainBowlersListAsync(context);
        }, "Loading bowlers...");
    }

    [RelayCommand]
    public async Task LoadSwitchBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            await SetSwitchBowlersListAsync(context);
        }, "Switching bowler...");
    }

    [RelayCommand]
    public async Task LoadBusRidesAsync()
    {
        await ExecuteAsync(async () =>
        {
            SetWorkingBusRideViewModelCommand.Execute(new());
            var busRides = await context.GetAllAsync<BusRide>();
            if (busRides.Any())
            {
                WorkingBusRideViewModel.BusRide = busRides.Last();
            }
            else
            {
                _ = await context.AddItemAsync(WorkingBusRideViewModel.BusRide);
            }
            var weeks = await context.GetFilteredAsync<BusRideWeek>(week => week.WeekNumber == WorkingWeek);
            if (weeks.Any())
            {
                WorkingBusRideViewModel.BusRideWeek = weeks.Last();
            }
            else
            {
                WorkingBusRideViewModel.BusRideWeek.BusRideId = WorkingBusRideViewModel.BusRide.Id;
                WorkingBusRideViewModel.BusRideWeek.WeekNumber = WorkingWeek;
                _ = await context.AddItemAsync(WorkingBusRideViewModel.BusRideWeek);
            }
        });
    }

    [RelayCommand]
    private void SetWorkingBowlerViewModel(BowlerViewModel viewModel) =>
        WorkingBowlerViewModel = viewModel ?? new();

    [RelayCommand]
    private void SetWorkingBusRideViewModel(BusRideViewModel viewModel) =>
        WorkingBusRideViewModel = viewModel ?? new();

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
        _ = await context.UpdateItemAsync(viewModel.Bowler);
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

    private void SetBusRideCounts()
    {
        BusRides = WorkingBusRideViewModel.BusRideWeek.BusRides;
        TotalBusRides = WorkingBusRideViewModel.BusRide.TotalBusRides;
    }

    private async Task SetMainBowlersListAsync(DatabaseContext context)
    {
        MainBowlers ??= [];
        var bowlers = await context.GetFilteredAsync<Bowler>(b => !b.IsHidden);
        var weeks = await context.GetFilteredAsync<BowlerWeek>(week => week.WeekNumber == WorkingWeek);
        if (bowlers is not null && bowlers.Any())
        {
            MainBowlers.Clear();
            MainBowlers = LoadBowlers(bowlers, weeks);
        }
    }

    private async Task SetSwitchBowlersListAsync(DatabaseContext context)
    {
        SwitchBowlers ??= [];
        var bowlers = await context.GetFilteredAsync<Bowler>(b => b.Id != WorkingBowlerViewModel.Bowler.Id && b.IsHidden);
        var weeks = await context.GetAllAsync<BowlerWeek>();
        if (bowlers is not null && bowlers.Any())
        {
            SwitchBowlers.Clear();
            SwitchBowlers = LoadBowlers(bowlers, weeks);
        }
    }

    private async Task SetWorkingWeekAsync(DatabaseContext context)
    {
        if (WorkingWeek == 0)
        {
            var allWeeks = await context.GetAllAsync<BowlerWeek>();
            WorkingWeek = allWeeks is not null && allWeeks.Any()
                ? allWeeks.OrderBy(w => w.WeekNumber).Last().WeekNumber
                : 1;
        }
    }
}
