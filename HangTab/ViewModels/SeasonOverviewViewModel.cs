using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Mappers;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels.Base;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class SeasonOverviewViewModel(
    IWeekService weekService,
    ISettingsService settingsService,
    INavigationService navigationService,
    IMapper<IEnumerable<Week>, IEnumerable<WeekListItemViewModel>> mapper) : ViewModelBase
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
            Weeks = mapper.Map(weeks.Where(w => w.Id != settingsService.CurrentWeekPrimaryKey).OrderByDescending(w => w.Number)).ToObservableCollection();
        }
    }
}
