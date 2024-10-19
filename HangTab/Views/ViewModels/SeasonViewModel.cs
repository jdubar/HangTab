using CommunityToolkit.Mvvm.Input;

using HangTab.Data;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public partial class SeasonViewModel(IDatabaseService data,
                                     IShellService shell) : BaseViewModel
{
    public ObservableRangeCollection<WeekViewModel> AllWeeks { get; set; } = [];

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        AllWeeks.ReplaceRange(await data.GetAllWeeks());
    }

    [RelayCommand]
    private async Task ShowWeekDetailsAsync(WeekViewModel week) => await shell.GoToPageWithDataAsync(nameof(WeekDetailsPage), week);
}
