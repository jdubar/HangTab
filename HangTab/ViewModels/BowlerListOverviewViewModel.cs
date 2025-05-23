using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Messages;
using HangTab.Services;
using HangTab.Services.Mappers;
using HangTab.ViewModels.Base;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class BowlerListOverviewViewModel :
    ViewModelBase,
    IRecipient<PersonAddedOrChangedMessage>,
    IRecipient<PersonDeletedMessage>,
    IRecipient<SystemResetMessage>
{
    private readonly IPersonService _personService;
    private readonly INavigationService _navigationService;
    private readonly IWeekService _weekService;

    public BowlerListOverviewViewModel(
        IPersonService personService,
        INavigationService navigationService,
        IWeekService weekService)
    {
        _personService = personService;
        _navigationService = navigationService;
        _weekService = weekService;

        WeakReferenceMessenger.Default.Register<PersonAddedOrChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<PersonDeletedMessage>(this);
        WeakReferenceMessenger.Default.Register<SystemResetMessage>(this);
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
            Task.Run(async () => { await SearchBowlers(value); }).Wait();
        }
    }

    [RelayCommand]
    private async Task NavigateToAddBowler() => await _navigationService.GoToAddBowler();

    [RelayCommand]
    private async Task NavigateToEditSelectedBowler()
    {
        if (SelectedBowler is not null)
        {
            await _navigationService.GoToEditBowler(SelectedBowler.MapBowlerListItemViewModelToBowler());
            SelectedBowler = null;
        }
    }

    public override async Task LoadAsync()
    {
        if (Bowlers.Count == 0)
        {
            await Loading(GetBowlers);
        }

        await UpdateBowlerHangCounts();
    }

    private async Task GetBowlers()
    {
        var people = await _personService.GetAllPeople();
        if (people.Any())
        {
            Bowlers.Clear();
            AllBowlers = people.OrderBy(b => b.Name).MapBowlerToBowlerListItemViewModel();
            Bowlers = AllBowlers.ToObservableCollection();
        }
    }

    private async Task UpdateBowlerHangCounts()
    {
        if (Bowlers.Count > 0)
        {
            var allWeeks = await _weekService.GetAllWeeks();
            if (allWeeks is null)
            {
                return;
            }

            Bowlers.ToList()
                   .ForEach(bowler => bowler.Hangings = allWeeks.SelectMany(w => w.Bowlers.Where(b => b.PersonId == bowler.Id))
                                                                .Sum(w => w.HangCount));
        }
    }

    private Task SearchBowlers(string searchText)
    {
        Bowlers.Clear();
        Bowlers = AllBowlers.Where(b => b.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToObservableCollection();
        return Task.CompletedTask;
    }

    public void Receive(SystemResetMessage message)
    {
        Bowlers.Clear();
        AllBowlers = [];
    }

    public async void Receive(PersonAddedOrChangedMessage message) => await UpdateBowlerList();

    public async void Receive(PersonDeletedMessage message) => await UpdateBowlerList();

    private async Task UpdateBowlerList()
    {
        Bowlers.Clear();
        await GetBowlers();
    }
}
