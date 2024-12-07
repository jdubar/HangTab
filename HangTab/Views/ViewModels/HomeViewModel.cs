﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HangTab.Data;
using HangTab.Extensions;
using HangTab.Models.Wrappers;
using MvvmHelpers;

namespace HangTab.Views.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public partial class HomeViewModel(
    IDatabaseService data,
    ISettingsService settings,
    IShellService shell,
    IAudioService audio,
    IWeekService weekService) : BaseViewModel
{
    // TODO: Add cumulative hang cost per bowler (maybe)
    // TODO: Notify user better somehow on new week
    // TODO: Add data reset button to season summary
    // TODO: Unit tests?
    // TODO: On bowlers view, add undo hanging option
    // TODO: Create reusable cardview
    // TODO: Change "Use sub" text based on whether a sub is used or not
    // TODO: Use sub's avatar to overlay main bowler's avatar

    //public ObservableRangeCollection<BowlerViewModel> MainBowlers { get; } = [];

    [ObservableProperty]
    private WeekWrapper _week;

    [ObservableProperty]
    private int _busRideTotal;



    [ObservableProperty]
    private bool _showBusRideImage;

    [ObservableProperty]
    private BusRideViewModel _busRideViewModel;

    [ObservableProperty]
    private bool _isUndoBusRideVisible;

    [ObservableProperty]
    private string _swipeText;

    private bool _isStartNewWeekVisible = true;

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        Title = $"Week {settings.CurrentSeasonWeek} of {settings.TotalSeasonWeeks}";
        Week = await GetCurrentWeek();
        BusRideTotal = await weekService.GetBusRideTotal();

        SetSwipeControlProperties();

        IsUndoBusRideVisible = IsBusRideGreaterThanZero();

        WeakReferenceMessenger.Default.Register<WeekUpdateMessage>(this, (_, handler) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Week.BusRideCount = handler.Value.BusRideCount;
                Week.WeekNumber = handler.Value.WeekNumber;
            });
        });
    }

    private async Task<WeekWrapper> GetCurrentWeek()
    {
        var week = await weekService.Get(settings.CurrentSeasonWeek);
        return new WeekWrapper(week);
    }

    [RelayCommand]
    private async Task BusRideAsync()
    {
        if (await weekService.AddBusRide(Week))
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
        viewModel.AddHanging();
        viewModel.BowlerWeek.WeekNumber = settings.CurrentSeasonWeek;
        viewModel.IsEnableUndo = true;
        viewModel.IsEnableSwitchBowler = false;

        if (await data.UpdateBowlerHangingsByWeek(viewModel, settings.CurrentSeasonWeek))
        {
            SetIsLowestHangsInMainBowlers();
        }
        else
        {
            await shell.DisplayAlertAsync("Update Error", "Error updating bowler hang count", "Ok");
        }
    }

    [RelayCommand]
    private async Task ShowSwitchBowlerViewAsync(Bowler bowler) => await shell.GoToPageWithDataAsync(nameof(SwitchBowlerPage), bowler);

    [RelayCommand]
    private async Task UndoBowlerHangAsync(BowlerViewModel viewModel)
    {
        if (viewModel.BowlerWeek.Hangings > 0)
        {
            viewModel.UndoHanging();
            viewModel.BowlerWeek.WeekNumber = settings.CurrentSeasonWeek;
            viewModel.IsEnableUndo = viewModel.BowlerWeek.Hangings != 0;
            viewModel.IsEnableSwitchBowler = viewModel.BowlerWeek.Hangings == 0;
        }
        else
        {
            viewModel.IsEnableUndo = false;
            viewModel.IsEnableSwitchBowler = true;
        }
        if (await data.UpdateBowlerHangingsByWeek(viewModel, settings.CurrentSeasonWeek))
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
        BusRideViewModel.UndoBusRide();
        IsUndoBusRideVisible = IsBusRideGreaterThanZero();

        if (!await data.UpdateBusRidesByWeek(BusRideViewModel, settings.CurrentSeasonWeek))
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

    private bool IsBusRideGreaterThanZero() => Week.BusRideCount > 0;

    private bool GetSliderState() => settings.CurrentSeasonWeek < settings.TotalSeasonWeeks;

    // TODO: Display only main bowlers on new week
    private void ResetMainBowlersForNewWeek() => MainBowlers.ResetForNewWeek(settings.CurrentSeasonWeek);

    private async Task ResetBusRidesForNewWeekAsync()
    {
        BusRideViewModel.ResetBusRidesForWeek(settings.CurrentSeasonWeek);
        if (await data.UpdateBusRidesByWeek(BusRideViewModel, settings.CurrentSeasonWeek))
        {
            await shell.DisplayToastAsync($"Now beginning week {settings.CurrentSeasonWeek}");
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
            if (!await data.UpdateBowlerHangingsByWeek(bowler, settings.CurrentSeasonWeek))
            {
                return;
            }
        }
    }

    private void SetIsLowestHangsInMainBowlers() => MainBowlers.SetIsLowestHangs();

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

            settings.CurrentSeasonWeek++;
            TitleWeek = $"Week {settings.CurrentSeasonWeek} of {settings.TotalSeasonWeeks}";

            SetSwipeControlProperties();

            ResetMainBowlersForNewWeek();
            await ResetBusRidesForNewWeekAsync();
        }, "Starting new week...");
    }
}
