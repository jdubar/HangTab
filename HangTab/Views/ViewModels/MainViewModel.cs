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
    private bool _showBusRideImage;

    [ObservableProperty]
    private int _totalBusRidesLabel;

    private BusRideViewModel BusRideViewModel { get; set; }

    private int WorkingWeek { get; set; }

    [RelayCommand]
    private async Task BusRideAsync()
    {
        await ExecuteAsync(async () =>
        {
            ShowBusRideImage = true;
            BusRideViewModel.BusRide.TotalBusRides++;
            BusRideViewModel.BusRideWeek.BusRides++;

            if (!await dbservice.UpdateBusRidesByWeek(BusRideViewModel, WorkingWeek))
            {
                await Shell.Current.DisplayAlert("Update Error", "Error updating bus ride", "Ok");
                return;
            }
            SetBusRideLabels();
            await Task.Delay(2000);
            ShowBusRideImage = false;
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

            if (!await dbservice.UpdateBowlerHangingsByWeek(viewModel, WorkingWeek))
            {
                await Shell.Current.DisplayAlert("Update Error", "Error updating bowler hang count", "Ok");
            }
        }, "Hanging bowler...");
    }

    public async Task LoadMainBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            if (WorkingWeek == 0)
            {
                WorkingWeek = await dbservice.GetWorkingWeek();
            }

            BusRideViewModel ??= await dbservice.GetLatestBusRide(WorkingWeek);

            SetBusRideLabels();

            SetWorkingBowlerViewModelCommand.Execute(new());
            await SetMainBowlersListAsync();
        }, "Loading bowlers...");
    }

    public async Task LoadSwitchBowlersAsync() =>
        await ExecuteAsync(SetSwitchBowlersListAsync,
                           "Loading bowlers...");

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
            if (!await ChangeBowlerHiddenStateAsync(WorkingBowlerViewModel))
            {
                await Shell.Current.DisplayAlert("Update Error", "Error updating bowler state", "Ok");
                return;
            }
            if (!await ChangeBowlerHiddenStateAsync(SelectedBowler))
            {
                await Shell.Current.DisplayAlert("Update Error", "Error updating bowler state", "Ok");
                return;
            }

            var index = MainBowlers.IndexOf(WorkingBowlerViewModel);
            MainBowlers.RemoveAt(index);
            MainBowlers.Insert(index, SelectedBowler);
        }, "Switching bowler...");

        await Shell.Current.GoToAsync("..", true);
    }

    private async Task<bool> ChangeBowlerHiddenStateAsync(BowlerViewModel viewModel)
    {
        viewModel.Bowler.IsHidden = !viewModel.Bowler.IsHidden;
        return await dbservice.UpdateBowler(viewModel.Bowler);
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

    private void SetBusRideLabels()
    {
        BusRidesLabel = BusRideViewModel.BusRideWeek.BusRides;
        TotalBusRidesLabel = BusRideViewModel.BusRide.TotalBusRides;
    }

    private async Task SetMainBowlersListAsync()
    {
        MainBowlers ??= [];
        var bowlers = await dbservice.GetFilteredBowlers(b => !b.IsHidden);
        var weeks = await dbservice.GetFilteredBowlerWeeks(WorkingWeek);
        if (bowlers is not null && bowlers.Any())
        {
            MainBowlers.Clear();
            MainBowlers = LoadBowlers(bowlers, weeks);
        }
    }

    private async Task SetSwitchBowlersListAsync()
    {
        SwitchBowlers ??= [];
        var bowlers = await dbservice.GetFilteredBowlers(b => b.Id != WorkingBowlerViewModel.Bowler.Id && b.IsHidden);
        var weeks = await dbservice.GetAllBowlerWeeks();
        if (bowlers is not null && bowlers.Any())
        {
            SwitchBowlers.Clear();
            SwitchBowlers = LoadBowlers(bowlers, weeks);
        }
    }
}
