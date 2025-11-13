using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Mappers;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels.Base;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class WeekDetailsViewModel(
    ISettingsService settingsService,
    IWeekService weekService) : ViewModelBase, IQueryAttributable
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _number;

    [ObservableProperty]
    private int _busRides;

    [ObservableProperty]
    private ObservableCollection<CurrentWeekListItemViewModel> _bowlers = [];

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private int _teamHangTotal;

    public override async Task LoadAsync()
    {
        await Loading(
            async () =>
            {
                if (Id > 0)
                {
                    await GetWeek(Id);
                }
            });

        InitializeCurrentWeekPageSettings();
    }

    private async Task GetWeek(int id)
    {
        var result = await weekService.GetByIdAsync(id);
        if (result.IsFailed)
        {
            return;
        }
        MapWeekData(result.Value);
    }

    private void InitializeCurrentWeekPageSettings()
    {
        TeamHangTotal = Bowlers.Sum(b => b.HangCount);
        PageTitle = $"Week {Number} of {settingsService.TotalSeasonWeeks}";
    }


    private void MapWeekData(Week week)
    {
        Id = week.Id;
        Number = week.Number;
        BusRides = week.BusRides;
        Bowlers = week.Bowlers.ToCurrentWeekListItemViewModelList().ToObservableCollection();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var weekId = query["weekId"] as int?;
        if (weekId.HasValue)
        {
            Id = weekId.Value;
        }
    }
}
