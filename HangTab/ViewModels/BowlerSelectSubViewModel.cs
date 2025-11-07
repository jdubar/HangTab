using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Mappers;
using HangTab.Messages;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels.Base;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class BowlerSelectSubViewModel(
    IPersonService personService,
    IBowlerService bowlerService,
    IMessenger messenger,
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
                    await GetSubsAsync();

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

        Subs.ToList().ForEach(sub => sub.IsSelected = false);
        SelectedSub.IsSelected = true;
    }

    [RelayCommand]
    private async Task SubmitSelectedSubAsync()
    {
        if (SelectedSub is null)
        {
            return;
        }

        await bowlerService.UpdateBowlerAsync(MapDataToBowler());
        messenger.Send(new BowlerSubChangedMessage(Id, SelectedSub.Id));
        await navigationService.GoBack();
    }

    private async Task GetSubsAsync()
    {
        var subs = await GetAvailableSubsAsync();
        if (subs.Any())
        {
            Subs.Clear();
            Subs = subs.ToSubListItemViewModelList().ToObservableCollection();
        }
    }

    private async Task<IEnumerable<Person>> GetAvailableSubsAsync()
    {
        var subs = await personService.GetSubstitutesAsync();
        if (!subs.Any())
        {
            return [];
        }

        var bowlers = await bowlerService.GetAllBowlersByWeekIdAsync(_bowler?.WeekId ?? 0);
        return bowlers.Any()
            ? subs.Where(s => !bowlers.Any(b => b.SubId == s.Id))
            : subs;
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
            _bowler = query[nameof(Bowler)] as Bowler;
        }
    }
}
