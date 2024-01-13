using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels;

using MvvmHelpers;

using Plugin.Maui.Audio;

namespace HangTab.Views.ViewModels;
public partial class MainViewModel(IDatabaseService data,
                                   IShellService shell,
                                   IAudioManager audio) : BaseViewModel
{
    public ObservableRangeCollection<BowlerViewModel> MainBowlers { get; set; } = [];

    [ObservableProperty]
    private bool _showBusRideImage;

    [ObservableProperty]
    private BusRideViewModel _busRideViewModel;

    [ObservableProperty]
    private string _titleWeek = "Week 0";

    private int WorkingWeek { get; set; }

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            WorkingWeek = await data.GetLatestWeek();
            TitleWeek = $"Week {WorkingWeek}";
            BusRideViewModel = await data.GetBusRideViewModelByWeek(WorkingWeek);

            MainBowlers.Clear();
            MainBowlers.AddRange(await data.GetMainBowlersByWeek(WorkingWeek));
        }, "");
    }

    [RelayCommand]
    private async Task BusRideAsync()
    {
        BusRideViewModel.BusRide.Total++;
        BusRideViewModel.BusRideWeek.BusRides++;

        if (await data.UpdateBusRidesByWeek(BusRideViewModel, WorkingWeek))
        {
            await ShowBusRideSplashAsync();
        }
        else
        {
            await shell.DisplayAlert("Update Error", "Error updating bus ride", "Ok");
        }
    }

    [RelayCommand]
    private async Task HangBowlerAsync(BowlerViewModel viewModel)
    {
        await ExecuteAsync(async () =>
        {
            viewModel.Bowler.TotalHangings++;
            viewModel.BowlerWeek.Hangings++;
            viewModel.BowlerWeek.WeekNumber = WorkingWeek;
            viewModel.IsEnableUndo = true;
            viewModel.IsEnableSwitch = false;

            if (await data.UpdateBowlerHangingsByWeek(viewModel, WorkingWeek))
            {
                SetIsLowestHangsInMainBowlers();
            }
            else
            {
                await shell.DisplayAlert("Update Error", "Error updating bowler hang count", "Ok");
            }
        }, "");
    }

    [RelayCommand]
    private async Task ShowSwitchBowlerViewAsync(Bowler bowler) =>
        await shell.GoToPageWithData(nameof(SwitchBowlerPage), bowler);

    [RelayCommand]
    private async Task StartNewWeekAsync()
    {
        await ExecuteAsync(async () =>
        {
            await SaveZeroHangBowlerLineup();

            WorkingWeek++;
            TitleWeek = $"Week {WorkingWeek}";

            ResetMainBowlersForNewWeek();
            await ResetBusRidesForNewWeek();
        }, "Starting new week...");
    }

    [RelayCommand]
    private async Task UndoBowlerHang(BowlerViewModel viewModel)
    {
        await ExecuteAsync(async () =>
        {
            if (viewModel.BowlerWeek.Hangings > 0)
            {
                viewModel.Bowler.TotalHangings--;
                viewModel.BowlerWeek.Hangings--;
                viewModel.BowlerWeek.WeekNumber = WorkingWeek;
                viewModel.IsEnableUndo = viewModel.BowlerWeek.Hangings != 0;
                viewModel.IsEnableSwitch = viewModel.BowlerWeek.Hangings == 0;
            }
            else
            {
                viewModel.IsEnableUndo = false;
                viewModel.IsEnableSwitch = true;
            }
            if (await data.UpdateBowlerHangingsByWeek(viewModel, WorkingWeek))
            {
                SetIsLowestHangsInMainBowlers();
            }
            else
            {
                await shell.DisplayAlert("Update Error", "Error updating bowler hang count", "Ok");
            }
        }, "");
    }

    private void ResetMainBowlersForNewWeek()
    {
        foreach (var bowler in MainBowlers)
        {
            bowler.BowlerWeek.Hangings = 0;
            bowler.BowlerWeek.WeekNumber = WorkingWeek;
            bowler.IsEnableSwitch = true;
            bowler.IsEnableUndo = false;
        }
    }

    private async Task ResetBusRidesForNewWeek()
    {
        BusRideViewModel.BusRideWeek.BusRides = 0;
        BusRideViewModel.BusRideWeek.WeekNumber = WorkingWeek;
        if (!await data.UpdateBusRidesByWeek(BusRideViewModel, WorkingWeek))
        {
            await shell.DisplayAlert("Update Error", "Error updating bus ride", "Ok");
        }
    }

    private async Task SaveZeroHangBowlerLineup()
    {
        foreach (var bowler in MainBowlers.Where(bowler => bowler.BowlerWeek.Hangings == 0))
        {
            _ = await data.UpdateBowlerHangingsByWeek(bowler, WorkingWeek);
        }
    }

    private void SetIsLowestHangsInMainBowlers()
    {
        foreach (var bowler in MainBowlers)
        {
            bowler.IsLowestHangs = !bowler.Bowler.IsSub
                                   && bowler.Bowler.TotalHangings == MainBowlers.Where(b => !b.Bowler.IsSub).Min(y => y.Bowler.TotalHangings);
        }
    }

    private async Task ShowBusRideSplashAsync()
    {
        ShowBusRideImage = true;
        using var player = audio.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("beep-beep.mp3"));
        player.Play();
        await Task.Delay(2000);
        ShowBusRideImage = false;
    }
}
