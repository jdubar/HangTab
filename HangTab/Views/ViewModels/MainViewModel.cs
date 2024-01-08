using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

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
    private int _busRides;

    [ObservableProperty]
    private int _busRideTotal;

    [ObservableProperty]
    private string _titleWeek = "Week 0";

    private Week Week { get; set; }

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            Week = await data.GetLatestWeek();
            BusRides = Week.BusRides;
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

        if (!await data.UpdateWeek(Week))
        {
            await shell.DisplayAlert("Update Error", "Error updating bus ride", "Ok");
        }
        else
        {
            await ShowBusRideSplashAsync();
        }
    }

    [RelayCommand]
    private async Task HangBowlerAsync(Bowler bowler)
    {
        await ExecuteAsync(async () =>
        {
            bowler.TotalHangings++;
            bowler.WeekHangings++;

            if (await data.UpdateBowler(bowler))
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
            await SaveCurrentWeek();

            Week = await data.StartNewWeek();
            TitleWeek = $"Week {Week.WeekNumber}";
            MainBowlers = Week.Bowlers.Where(b => !b.IsHidden) as ObservableRangeCollection<Bowler>;

        }, "Starting new week...");
    }

    private async Task SaveCurrentWeek()
    {
        await data.UpdateAllBowlers(MainBowlers);
        await data.UpdateTotalHangs(Week.BusRides);
    }

    private void SetIsLowestHangsInMainBowlers()
    {
        foreach (var bowler in MainBowlers)
        {
            bowler.IsLowestHangs = !bowler.IsSub
                                   && bowler.TotalHangings == MainBowlers.Where(b => !b.IsSub).Min(y => y.TotalHangings);
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
