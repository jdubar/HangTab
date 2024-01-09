using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;

[QueryProperty(nameof(Week), nameof(Week))]
public partial class WeekDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    private Week _week;

    [ObservableProperty]
    private string _titleWeek = "Week 0 Details";

    public ObservableRangeCollection<Bowler> BowlersList { get; set; } = [];

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        await ExecuteAsync(() => {
            TitleWeek = $"Week {Week.WeekNumber} Details";
            BowlersList.Clear();

            if (Week.Bowlers.Any())
            {
                BowlersList.AddRange(Week.Bowlers);
            }

            return Task.CompletedTask;
        }, "");
    }
}
