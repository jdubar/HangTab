using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Models;
using HangTab.Services;
using HangTab.Services.Mappers;
using HangTab.ViewModels.Base;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class BowlerSwitchViewModel(IBowlerService bowlerService) :
    ViewModelBase,
    IQueryAttributable
{
    private WeeklyLineup? _bowler;

    [ObservableProperty]
    private ObservableCollection<BowlerListItemViewModel> _subs = [];

    public override async Task LoadAsync()
    {
        if (Subs.Count == 0)
        {
            await Loading(GetBowlers);
        }
    }

    private async Task GetBowlers()
    {
        var subs = await bowlerService.GetSubstituteBowlers();
        Subs = subs.MapBowlerToBowlerListItemViewModel().ToObservableCollection();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            _bowler = query["WeeklyLineup"] as WeeklyLineup;
        }
    }
}
