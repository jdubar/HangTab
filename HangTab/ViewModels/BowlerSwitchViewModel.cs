using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Enums;
using HangTab.Models;
using HangTab.Services;
using HangTab.Services.Mappers;
using HangTab.ViewModels.Base;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class BowlerSwitchViewModel(
    IPersonService personService,
    IBowlerService bowlerService) :
    ViewModelBase,
    IQueryAttributable
{
    private Bowler? _bowler;

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private ObservableCollection<BowlerListItemViewModel> _subs = [];

  	[ObservableProperty]
    private Status _selectedStatus;

    [ObservableProperty]
    private BowlerListItemViewModel? _selectedSub;

    [ObservableProperty]
    private bool _isSubsListVisible = true;

    public IReadOnlyList<Status> AllStatuses { get; } = Enum.GetValues<Status>().ToList();

    public override async Task LoadAsync()
    {
        await Loading(
            async () =>
            {
                if (_bowler is null && Id > 0)
                {
                    _bowler = await bowlerService.GetBowlerById(Id);
                }

                //if (_bowler is not null)
                //{
                //    IsExistingBowler = true;
                //}

                //MapBowler(_bowler);
            }
        );

        if (Subs.Count == 0)
        {
            await Loading(GetBowlers);
        }
    }

    private async Task GetBowlers()
    {
        var subs = await personService.GetSubstitutes();
        Subs = subs.MapBowlerToBowlerListItemViewModel().ToObservableCollection();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            _bowler = query["Bowler"] as Bowler;
        }
    }
}
