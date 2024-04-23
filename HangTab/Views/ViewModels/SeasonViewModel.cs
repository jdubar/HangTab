using CommunityToolkit.Mvvm.Input;

using HangTab.Services;
using HangTab.ViewModels;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
public partial class SeasonViewModel(IDatabaseService data,
                                     IShellService shell) : BaseViewModel
{
    public ObservableRangeCollection<WeekViewModel> AllWeeks { get; set; } = [];

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        AllWeeks.Clear();
        AllWeeks.AddRange(await data.GetAllWeeks());
    }

    [RelayCommand]
    private async Task ShowWeekDetailsAsync(WeekViewModel week) =>
        await shell.GoToPageWithData(nameof(WeekDetailsPage), week);
}
