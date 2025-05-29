using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Messages;
using HangTab.Models;
using HangTab.Services;
using HangTab.Services.Mappers;
using HangTab.ViewModels.Base;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class BowlerSelectSubViewModel(
    IPersonService personService,
    IBowlerService bowlerService,
    INavigationService navigationService) : ViewModelBase, IQueryAttributable
{
    private Bowler? _bowler;

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EnableSubmitButton))]
    private SubListItemViewModel? _selectedSub;

    [ObservableProperty]
    private ObservableCollection<SubListItemViewModel> _subs = [];

    public bool EnableSubmitButton => SelectedSub is not null;

    public override async Task LoadAsync()
    {
        if (Subs.Count == 0)
        {
            await Loading(
                async () =>
                {
                    await GetSubs();

                    if (_bowler is not null)
                    {
                        Id = _bowler.Id;
                        SelectedSub = Subs.FirstOrDefault(s => s.Id == _bowler.SubId);
                        if (SelectedSub is not null)
                        {
                            SelectedSub.IsSelected = true;
                        }
                    }
                });
        }

    }

    [RelayCommand]
    private void ShowCheckmarkOnSelectedSub()
    {
        if (SelectedSub is null)
        {
            return;
        }

        DeselectAllSubs();
        SelectedSub.IsSelected = true;
    }

    [RelayCommand]
    private async Task SubmitSelectedSubAsync()
    {
        if (SelectedSub is null)
        {
            return;
        }

        await bowlerService.UpdateBowler(MapDataToBowler());
        WeakReferenceMessenger.Default.Send(new BowlerSubChangedMessage(Id, SelectedSub.Id));
        await navigationService.GoBack();
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

    private Bowler MapDataToBowler()
    {
        return new Bowler
        {
            Id = _bowler?.Id ?? 0,
            WeekId = _bowler?.WeekId ?? 0,
            PersonId = _bowler?.PersonId ?? 0,
            SubId = SelectedSub?.Id,
            Status = _bowler?.Status ?? Enums.Status.UsingSub,
            HangCount = _bowler?.HangCount ?? 0
        };
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            _bowler = query["Bowler"] as Bowler;
        }
    }
}
