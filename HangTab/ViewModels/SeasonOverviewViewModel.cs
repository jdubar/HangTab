using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Services;
using HangTab.Services.Mappers;
using HangTab.ViewModels.Base;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class SeasonOverviewViewModel(
    IWeekService weekService,
    INavigationService navigationService,
    ISettingsService settingsService) : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<WeekListItemViewModel> _weeks = [];

    [ObservableProperty]
    private WeekListItemViewModel? _selectedWeek;

    public override async Task LoadAsync()
    {
        await Loading(GetWeeks);
    }

    private async Task GetWeeks()
    {
        var weeks = await weekService.GetAllWeeks();
        if (weeks.Any())
        {
            Weeks.Clear();
            Weeks = weeks.Where(w => w.Id != settingsService.CurrentWeekPrimaryKey)
                         .OrderByDescending(b => b.Number)
                         .MapWeekToWeekListItemViewModel()
                         .ToObservableCollection();
        }
    }
}
