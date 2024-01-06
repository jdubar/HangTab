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
            if (WorkingWeek == 0)
            {
                WorkingWeek = await data.GetWorkingWeek();
            }
            TitleWeek = $"Week {WorkingWeek}";
            BusRideViewModel = await data.GetBusRideViewModelByWeek(WorkingWeek);

            var bowlers = await data.GetFilteredBowlers(b => !b.IsHidden);
            var weeks = await data.GetFilteredBowlerWeeks(WorkingWeek);

            MainBowlers.Clear();

            if (bowlers.Any())
            {
                MainBowlers.AddRange(LoadBowlers(bowlers, weeks));
            }
        }, "");
    }

    [RelayCommand]
    private async Task BusRideAsync()
    {
        BusRideViewModel.BusRide.TotalBusRides++;
        BusRideViewModel.BusRideWeek.BusRides++;

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

    private List<BowlerViewModel> LoadBowlers(IEnumerable<Bowler> bowlers, IEnumerable<BowlerWeek> weeks)
    {
        var collection = new List<BowlerViewModel>();
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
