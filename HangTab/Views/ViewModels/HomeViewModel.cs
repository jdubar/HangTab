﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HangTab.Data;
using MvvmHelpers;

namespace HangTab.Views.ViewModels;
public partial class HomeViewModel(IDatabaseService data,
                                   IShellService shell,
                                   IAudioService audio) : BaseViewModel
{
    // TODO: Add cumulative hang cost per bowler (maybe)
    // TODO: Notify user better somehow on new week
    // TODO: Add data reset button to season summary
    // TODO: Unit tests?
    // TODO: On bowlers view, add undo hanging option
    // TODO: Create reusable cardview

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
    private bool _isUndoBusRideVisible;

    [ObservableProperty]
    private string _swipeText;

    private bool _isStartNewWeekVisible = true;
    private int WorkingWeek { get; set; }

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        SeasonSettings = await data.GetSeasonSettings();
        WorkingWeek = await data.GetLatestWeek();
        TitleWeek = $"Week {WorkingWeek} of {SeasonSettings.TotalSeasonWeeks}";
        BusRideViewModel = await data.GetBusRideViewModelByWeek(WorkingWeek);

        SetSwipeControlProperties();

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
            ShowBusRideImage = true;
            audio.PlayBusRideSound();
            ShowBusRideImage = false;
        }
        else
        {
            await shell.DisplayAlertAsync("Update Error", "Error updating bus ride", "Ok");
        }
    }

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
    private async Task ShowSwitchBowlerViewAsync(Bowler bowler)
        => await shell.GoToPageWithDataAsync(nameof(SwitchBowlerPage), bowler);

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

    [RelayCommand]
    private async Task ExecuteSlideAsync()
    {
        if (_isStartNewWeekVisible)
        {
            await StartNewWeekAsync();
        }
        else
        {
            await shell.GoToPageAsync(nameof(SeasonSummaryPage));
        }
    }

    private bool IsBusRideGreaterThanZero()
        => BusRideViewModel.BusRideWeek.BusRides > 0;

    private bool GetSliderState()
        => WorkingWeek < SeasonSettings.TotalSeasonWeeks;

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

    private void SetSwipeControlProperties()
    {
        _isStartNewWeekVisible = GetSliderState();
        SwipeText = _isStartNewWeekVisible
            ? "Swipe to save and start a new week"
            : "Swipe for the season summary!";
    }

    private async Task StartNewWeekAsync()
    {
        await ExecuteAsync(async () =>
        {
            await SaveZeroHangBowlerLineupAsync();

            WorkingWeek++;
            TitleWeek = $"Week {WorkingWeek} of {SeasonSettings.TotalSeasonWeeks}";

            SetSwipeControlProperties();

            ResetMainBowlersForNewWeek();
            await ResetBusRidesForNewWeekAsync();
        }, "Starting new week...");
    }
}
