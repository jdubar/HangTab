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
    private bool _showBusRideImage;

    [ObservableProperty]
    private BusRideViewModel _busRideViewModel;

    private int WorkingWeek { get; set; }

    public async Task InitializeDataAsync()
    {
        if (WorkingWeek == 0)
        {
            WorkingWeek = await data.GetWorkingWeek();
        }
        BusRideViewModel = await data.GetLatestBusRide(WorkingWeek);
        await ExecuteAsync(SetMainBowlersListAsync, "Loading bowlers...");
    }

    [RelayCommand]
    private async Task BusRideAsync()
    {
        await ExecuteAsync(async () =>
        {
            BusRideViewModel.BusRide.TotalBusRides++;
            BusRideViewModel.BusRideWeek.BusRides++;

            if (!await data.UpdateBusRidesByWeek(BusRideViewModel, WorkingWeek))
            {
                await shell.DisplayAlert("Update Error", "Error updating bus ride", "Ok");
                return;
            }
            else
            {
                ShowBusRideImage = true;
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

    [RelayCommand]
    private async Task ShowSwitchBowlerViewAsync(Bowler bowler) =>
        await shell.GoToPageWithData(nameof(SwitchBowlerPage), bowler);

    [RelayCommand]
    private async Task StartNewWeekAsync()
    {
        await ExecuteAsync(() =>
        {
            WorkingWeek++;
            foreach (var week in MainBowlers.Select(b => b.BowlerWeek))
            {
                week.Hangings = 0;
                week.WeekNumber = WorkingWeek;
            }
            BusRideViewModel.BusRideWeek.BusRides = 0;
            BusRideViewModel.BusRideWeek.WeekNumber = WorkingWeek;
            return Task.CompletedTask;
        }, "Starting new week...");
    }

    private ObservableCollection<BowlerViewModel> LoadBowlers(IEnumerable<Bowler> bowlers, IEnumerable<BowlerWeek> weeks)
    {
        var collection = new ObservableCollection<BowlerViewModel>();
        var lowest = bowlers.Where(b => !b.IsSub && b.TotalHangings == bowlers.Where(b => !b.IsSub).Min(b => b.TotalHangings));

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
        var bowlers = await data.GetFilteredBowlers(b => !b.IsHidden);
        var weeks = await data.GetFilteredBowlerWeeks(WorkingWeek);
        MainBowlers = bowlers is not null && bowlers.Any()
            ? LoadBowlers(bowlers, weeks)
            : [];
    }
}
