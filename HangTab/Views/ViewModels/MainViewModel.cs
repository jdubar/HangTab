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

            _ = await context.UpdateItemAsync(BusRideViewModel.BusRide);
            var busRides = await context.GetFilteredAsync<BusRideWeek>(b => b.WeekNumber == WorkingWeek);
            _ = busRides is not null && busRides.Any()
                ? await context.UpdateItemAsync(BusRideViewModel.BusRideWeek)
                : await context.AddItemAsync(BusRideViewModel.BusRideWeek);

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

            _ = await context.UpdateItemAsync(viewModel.Bowler);

            var weeks = await context.GetFilteredAsync<BowlerWeek>(w => w.WeekNumber == WorkingWeek
                                                                        && w.BowlerId == viewModel.Bowler.Id);
            _ = weeks is not null && weeks.Any()
                ? await context.UpdateItemAsync(viewModel.BowlerWeek)
                : await context.AddItemAsync(viewModel.BowlerWeek);
        }, "Hanging bowler...");
    }

    public async Task LoadMainBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            await SetWorkingWeekAsync(context);
            await LoadBusRidesAsync();

            SetBusRideLabels();

            SetWorkingBowlerViewModelCommand.Execute(new());
            await SetMainBowlersListAsync(context);
        }, "Loading bowlers...");
    }

    public async Task LoadSwitchBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            await SetSwitchBowlersListAsync(context);
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

    private async Task LoadBusRidesAsync()
    {
        await ExecuteAsync(async () =>
        {
            BusRideViewModel = new();
            var busRides = await context.GetAllAsync<BusRide>();
            if (busRides.Any())
            {
                BusRideViewModel.BusRide = busRides.Last();
            }
            else
            {
                _ = await context.AddItemAsync(BusRideViewModel.BusRide);
            }
            var weeks = await context.GetFilteredAsync<BusRideWeek>(week => week.WeekNumber == WorkingWeek);
            if (weeks.Any())
            {
                BusRideViewModel.BusRideWeek = weeks.Last();
            }
            else
            {
                BusRideViewModel.BusRideWeek.BusRideId = BusRideViewModel.BusRide.Id;
                BusRideViewModel.BusRideWeek.WeekNumber = WorkingWeek;
                _ = await context.AddItemAsync(BusRideViewModel.BusRideWeek);
            }
        });
    }

    private void SetBusRideLabels()
    {
        BusRidesLabel = BusRideViewModel.BusRideWeek.BusRides;
        TotalBusRidesLabel = BusRideViewModel.BusRide.TotalBusRides;
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
