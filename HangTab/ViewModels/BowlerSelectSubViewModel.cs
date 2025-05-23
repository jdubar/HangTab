using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;
using HangTab.Services.Mappers;
using HangTab.ViewModels.Base;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class BowlerSelectSubViewModel(IPersonService personService) : ViewModelBase, IQueryAttributable
{
    private Bowler? _bowler;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EnabelSubmitButton))]
    private SubListItemViewModel? _selectedSub = null;

    [ObservableProperty]
    private ObservableCollection<SubListItemViewModel> _subs = [];

    public bool EnabelSubmitButton => SelectedSub is not null;

    public override async Task LoadAsync()
    {
        if (Subs.Count == 0)
        {
            await Loading(GetSubs);
        }
    }

    [RelayCommand]
    private async Task ShowCheckmarkOnSelectedSub()
    {
        if (SelectedSub is null)
        {
            return;
        }

        DeselectAllSubs();
        SelectedSub.IsSelected = true;
    }

    private void DeselectAllSubs()
    {
        foreach (var sub in Subs)
        {
            sub.IsSelected = false;
        }
    }

    private async Task GetSubs()
    {
        var subs = await personService.GetSubstitutes();
        if (subs.Any())
        {
            Subs.Clear();
            Subs = subs.MapPersonToSubListItemViewModel().ToObservableCollection();
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            _bowler = query["Bowler"] as Bowler;
        }
    }
}
