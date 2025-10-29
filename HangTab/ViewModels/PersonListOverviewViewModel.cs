using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Extensions;
using HangTab.Mappers;
using HangTab.Messages;
using HangTab.Services;
using HangTab.ViewModels.Base;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class PersonListOverviewViewModel :
    ViewModelBase,
    IRecipient<BowlerHangCountChangedMessage>,
    IRecipient<PersonAddedOrChangedMessage>,
    IRecipient<PersonDeletedMessage>,
    IRecipient<SystemResetMessage>
{
    private readonly IMessenger _messenger;
    private readonly INavigationService _navigationService;
    private readonly IPersonService _personService;
    private readonly IWeekService _weekService;

    public PersonListOverviewViewModel(
        IMessenger messenger,
        INavigationService navigationService,
        IPersonService personService,
        IWeekService weekService)
    {
        _messenger = messenger;
        _navigationService = navigationService;
        _personService = personService;
        _weekService = weekService;

        _messenger.Register<BowlerHangCountChangedMessage>(this);
        _messenger.Register<PersonAddedOrChangedMessage>(this);
        _messenger.Register<PersonDeletedMessage>(this);
        _messenger.Register<SystemResetMessage>(this);
    }

    private IEnumerable<BowlerListItemViewModel> _allBowlers = [];
    private IEnumerable<BowlerListItemViewModel> AllBowlers
    {
        get => _allBowlers;
        set
        {
            SetProperty(ref _allBowlers, value);
            if (!_allBowlers.Any())
            {
                Bowlers.Clear();
            }
        }
    }

    [ObservableProperty]
    private ObservableCollection<BowlerListItemViewModel> _bowlers = [];

    [ObservableProperty]
    private BowlerListItemViewModel? _selectedBowler;

    [ObservableProperty]
    private string _searchText = string.Empty;
    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Bowlers.Clear();
            Bowlers = AllBowlers.OrderBy(b => b.Name).ToObservableCollection();
        }
        else
        {
            SearchBowlers(value);
        }
    }

    // TODO: Add filtering by Subs,Regulars (using chips?)
    // TODO: Add sort by most hangs,least hangs, name asc/desc

    [RelayCommand]
    private async Task NavigateToAddBowlerAsync() => await _navigationService.GoToAddBowler();

    [RelayCommand]
    private async Task NavigateToEditSelectedBowlerAsync()
    {
        if (SelectedBowler is not null)
        {
            await _navigationService.GoToEditBowler(SelectedBowler.ToPerson());
            SelectedBowler = null;
        }
    }

    public override async Task LoadAsync()
    {
        if (Bowlers.Count == 0)
        {
            await Loading(GetBowlersAsync);
        }
    }

    private async Task GetBowlersAsync()
    {
        var people = await _personService.GetAllPeople();
        if (!people.Any())
        {
            return;
        }

        Bowlers.Clear();
        AllBowlers = people.OrderBy(b => b.Name).ToBowlerListItemViewModelList();
        Bowlers = AllBowlers.ToObservableCollection();

        await UpdateBowlerHangCountsAsync();
    }

    private async Task UpdateBowlerHangCountsAsync()
    {
        if (Bowlers.Count > 0)
        {
            var allWeeks = await _weekService.GetAllWeeks();
            if (allWeeks is null)
            {
                return;
            }

            Bowlers.SetBowlerHangSumByWeeks(allWeeks);
            Bowlers.SetLowestBowlerHangCount();
        }
    }

    private void SearchBowlers(string searchText)
    {
        Bowlers.Clear();
        Bowlers = AllBowlers.Where(b => b.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToObservableCollection();
    }

    public void Receive(SystemResetMessage message)
    {
        Bowlers.Clear();
        AllBowlers = [];
    }

    public async void Receive(BowlerHangCountChangedMessage message) => await UpdateBowlerHangCountsAsync();

    public async void Receive(PersonAddedOrChangedMessage message) => await GetBowlersAsync();

    public async void Receive(PersonDeletedMessage message) => await GetBowlersAsync();
}
