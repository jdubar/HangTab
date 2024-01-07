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
    public ObservableRangeCollection<Bowler> MainBowlers { get; set; } = [];

    [ObservableProperty]
    private bool _showBusRideImage;

    [ObservableProperty]
    private BusRideViewModel _busRideViewModel;

    [ObservableProperty]
    private int _busRideTotal;

    [ObservableProperty]
    private string _titleWeek = "Week 0";

    private Week Week { get; set; }

    private int WorkingWeek { get; set; }

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            Week ??= await data.GetLatestWeek();
            BusRideTotal = await data.GetTotalBusRides();

            TitleWeek = $"Week {Week.WeekNumber}";

            MainBowlers.Clear();
            if (Week.Bowlers.Any())
            {
                MainBowlers.AddRange(Week.Bowlers);
            }
        }, "");
    }

    [RelayCommand]
    private async Task BusRideAsync()
    {
        BusRideTotal++;
        Week.BusRides++;

        if (!await data.UpdateBusRidesByWeek(BusRideViewModel, WorkingWeek))
        {
            await shell.DisplayAlert("Update Error", "Error updating bus ride", "Ok");
        }
        else
        {
            await ShowBusRideSplashAsync();
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

    private List<BowlerViewModel> LoadBowlers()
    {
        var collection = new List<BowlerViewModel>();
        var lowest = Week.Bowlers.Where(b => !b.IsSub && b.TotalHangings == Week.Bowlers.Where(b => !b.IsSub).Min(b => b.TotalHangings));

        foreach (var bowler in Week.Bowlers)
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

    private void ResetMainBowlersForNewWeek()
    {
        foreach (var week in MainBowlers.Select(b => b.BowlerWeek))
        {
            week.Hangings = 0;
            week.WeekNumber = WorkingWeek;
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
