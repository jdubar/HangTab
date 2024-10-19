using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Extensions;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
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
            BowlersList.AddBowlersToCollection(WeekViewModel.Bowlers);

            return Task.CompletedTask;
        }, "");
    }
}
