using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.ViewModels;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;

[QueryProperty(nameof(WeekViewModel), nameof(WeekViewModel))]
public partial class WeekDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    private WeekViewModel _weekViewModel;

    [ObservableProperty]
    private string _titleWeek = "Week 0 Details";

    public ObservableRangeCollection<Bowler> BowlersList { get; set; } = [];

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        await ExecuteAsync(() => {
            TitleWeek = $"Week {WeekViewModel.WeekNumber} Details";
            BowlersList.Clear();
            BowlersList.AddRange(WeekViewModel.Bowlers);

            return Task.CompletedTask;
        }, "");
    }
}
