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
    // TODO: Add cumulative hang cost per bowler
    // TODO: Notify user somehow on new week

    public ObservableRangeCollection<BowlerViewModel> MainBowlers { get; } = [];

    [ObservableProperty]
    private bool _showBusRideImage;

    [ObservableProperty]
    private BusRideViewModel _busRideViewModel;

    [ObservableProperty]
    private string _titleWeek;

    [ObservableProperty]
    private SeasonSettings _seasonSettings;

    [ObservableProperty]
    private bool _isSliderVisible = true;

    [ObservableProperty]
    private bool _isShowSummaryVisible;

    [ObservableProperty]
    private bool _isUndoBusRideVisible;

    private int WorkingWeek { get; set; }

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        SeasonSettings = await data.GetSeasonSettings();
        WorkingWeek = await data.GetLatestWeek();
        TitleWeek = $"Week {WorkingWeek} of {SeasonSettings.TotalSeasonWeeks}";
        BusRideViewModel = await data.GetBusRideViewModelByWeek(WorkingWeek);

        IsSliderVisible = IsStartNewWeekVisible();
        IsShowSummaryVisible = !IsSliderVisible;
        IsUndoBusRideVisible = IsBusRideGreaterThanZero();

        MainBowlers.ReplaceRange(await data.GetMainBowlersByWeek(WorkingWeek));
    }

    [RelayCommand]
    private async Task BusRideAsync()
    {
        BusRideViewModel.BusRide.Total++;
        BusRideViewModel.BusRideWeek.BusRides++;

        if (await data.UpdateBusRidesByWeek(BusRideViewModel, WorkingWeek))
        {
            IsUndoBusRideVisible = true;
            await ShowBusRideSplashAsync();
        }
        else
        {
            await shell.DisplayAlertAsync("Update Error", "Error updating bus ride", "Ok");
        }
    }

    [RelayCommand]
    private async Task ShowSeasonSummaryViewAsync() =>
        await shell.GoToPageAsync(nameof(SeasonSummaryPage));

    [RelayCommand]
    private async Task HangBowlerAsync(BowlerViewModel viewModel)
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
            await shell.DisplayAlertAsync("Update Error", "Error updating bowler hang count", "Ok");
        }
    }

    [RelayCommand]
    private async Task ShowSwitchBowlerViewAsync(Bowler bowler) =>
        await shell.GoToPageWithDataAsync(nameof(SwitchBowlerPage), bowler);

    [RelayCommand]
    private async Task StartNewWeekAsync()
    {
        await ExecuteAsync(async () =>
        {
            await SaveZeroHangBowlerLineupAsync();

            WorkingWeek++;
            TitleWeek = $"Week {WorkingWeek} of {SeasonSettings.TotalSeasonWeeks}";

            IsSliderVisible = IsStartNewWeekVisible();
            IsShowSummaryVisible = !IsSliderVisible;

            ResetMainBowlersForNewWeek();
            await ResetBusRidesForNewWeekAsync();
        }, "Starting new week...");
    }

    [RelayCommand]
    private async Task UndoBowlerHangAsync(BowlerViewModel viewModel)
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
            await shell.DisplayAlertAsync("Update Error", "Error updating bowler hang count", "Ok");
        }
    }

    [RelayCommand]
    private async Task UndoBusRideAsync()
    {
        BusRideViewModel.BusRide.Total--;
        BusRideViewModel.BusRideWeek.BusRides--;

        IsUndoBusRideVisible = IsBusRideGreaterThanZero();

        if (!await data.UpdateBusRidesByWeek(BusRideViewModel, WorkingWeek))
        {
            await shell.DisplayAlertAsync("Update Error", "Error updating bus ride", "Ok");
        }
    }

    private bool IsBusRideGreaterThanZero() =>
        BusRideViewModel.BusRideWeek.BusRides > 0;

    private bool IsStartNewWeekVisible() =>
        WorkingWeek < SeasonSettings.TotalSeasonWeeks;

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

    private async Task ResetBusRidesForNewWeekAsync()
    {
        BusRideViewModel.BusRideWeek.BusRides = 0;
        BusRideViewModel.BusRideWeek.WeekNumber = WorkingWeek;
        if (await data.UpdateBusRidesByWeek(BusRideViewModel, WorkingWeek))
        {
            await shell.DisplayToastAsync($"Now beginning week {WorkingWeek}");
        }
        else
        {
            await shell.DisplayAlertAsync("Update Error", "Error updating bus ride", "Ok");
        }
    }

    private async Task SaveZeroHangBowlerLineupAsync()
    {
        foreach (var bowler in MainBowlers.Where(bowler => bowler.BowlerWeek.Hangings == 0))
        {
            if (!await data.UpdateBowlerHangingsByWeek(bowler, WorkingWeek))
            {
                return;
            }
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
