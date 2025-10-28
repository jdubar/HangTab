using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Mappers;
using HangTab.Services;
using HangTab.ViewModels.Base;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class WeekListOverviewViewModel(
    IWeekService weekService,
    ISettingsService settingsService,
    INavigationService navigationService) : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<WeekListItemViewModel> _weeks = [];

    [ObservableProperty]
    private WeekListItemViewModel? _selectedWeek;

    public override async Task LoadAsync() => await Loading(GetWeeks);

    private async Task GetWeeks()
    {
        var weeks = await weekService.GetAllWeeks();
        if (!weeks.Any())
        {
            return;
        }

        Weeks.Clear();
        Weeks = weeks.Where(w => w.Id != settingsService.CurrentWeekPrimaryKey).OrderByDescending(w => w.Number).ToWeekListItemViewModelList().ToObservableCollection();
    }

    [RelayCommand]
    private async Task NavigateToSelectedWeekDetails()
    {
        if (SelectedWeek is not null)
        {
            await navigationService.GoToWeekDetails(SelectedWeek.Id);
            SelectedWeek = null;
        }
    }
}
