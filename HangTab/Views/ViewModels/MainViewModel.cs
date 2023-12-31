using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels;

using System.Collections.ObjectModel;

namespace HangTab.Views.ViewModels;
public partial class MainViewModel(IDatabaseService data, IShellService shell) : BaseViewModel
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
    private bool _showBusRideImage;

    [ObservableProperty]
    private BusRideViewModel _busRideViewModel;

    private int WorkingWeek { get; set; }

    [RelayCommand]
    private async Task BusRideAsync()
    {
        await ExecuteAsync(async () =>
        {
            ShowBusRideImage = true;
            BusRideViewModel.BusRide.TotalBusRides++;
            BusRideViewModel.BusRideWeek.BusRides++;

            if (!await data.UpdateBusRidesByWeek(BusRideViewModel, WorkingWeek))
            {
                await shell.DisplayAlert("Update Error", "Error updating bus ride", "Ok");
                return;
            }
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

            if (!await data.UpdateBowlerHangingsByWeek(viewModel, WorkingWeek))
            {
                await shell.DisplayAlert("Update Error", "Error updating bowler hang count", "Ok");
            }
            else
            {
                foreach (var bowler in MainBowlers)
                {
                    bowler.IsLowestHangs = !bowler.Bowler.IsSub
                                           && bowler.Bowler.TotalHangings == MainBowlers.Min(y => y.Bowler.TotalHangings);
                }
            }
        }, "Hanging bowler...");
    }

    public async Task LoadMainBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            if (WorkingWeek == 0)
            {
                WorkingWeek = await data.GetWorkingWeek();
            }

            BusRideViewModel = await data.GetLatestBusRide(WorkingWeek);

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
        await shell.GoToPage(nameof(SwitchBowlerPage));
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
                await shell.DisplayAlert("Update Error", "Error updating bowler state", "Ok");
                return;
            }
            if (!await ChangeBowlerHiddenStateAsync(SelectedBowler))
            {
                await shell.DisplayAlert("Update Error", "Error updating bowler state", "Ok");
                return;
            }

            var index = MainBowlers.IndexOf(WorkingBowlerViewModel);
            MainBowlers.RemoveAt(index);
            MainBowlers.Insert(index, SelectedBowler);
        }, "Switching bowler...");

        await shell.ReturnToPage();
    }

    private async Task<bool> ChangeBowlerHiddenStateAsync(BowlerViewModel viewModel)
    {
        viewModel.Bowler.IsHidden = !viewModel.Bowler.IsHidden;
        return await data.UpdateBowler(viewModel.Bowler);
    }

    private async Task<ObservableCollection<BowlerViewModel>> LoadBowlers(IEnumerable<Bowler> bowlers, IEnumerable<BowlerWeek> weeks)
    {
        var collection = new ObservableCollection<BowlerViewModel>();
        var lowest = await data.GetLowestHangs();

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
            if (lowest.Any(b => b.Id == bowler.Id))
            {
                viewModel.IsLowestHangs = true;
            }
            collection.Add(viewModel);
        }
        return collection;
    }

    private async Task SetMainBowlersListAsync()
    {
        MainBowlers ??= [];
        var bowlers = await data.GetFilteredBowlers(b => !b.IsHidden);
        var weeks = await data.GetFilteredBowlerWeeks(WorkingWeek);
        MainBowlers = bowlers is not null && bowlers.Any()
            ? await LoadBowlers(bowlers, weeks)
            : ([]);
    }

    private async Task SetSwitchBowlersListAsync()
    {
        SwitchBowlers ??= [];
        var bowlers = await data.GetFilteredBowlers(b => b.Id != WorkingBowlerViewModel.Bowler.Id && b.IsHidden);
        var weeks = await data.GetAllBowlerWeeks();
        SwitchBowlers = bowlers is not null && bowlers.Any()
            ? await LoadBowlers(bowlers, weeks)
            : ([]);
    }
}
