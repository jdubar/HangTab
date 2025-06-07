using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Mappers;

using HangTab.Messages;
using HangTab.Services;
using HangTab.ViewModels.Base;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class BowlerOverviewViewModel :
    ViewModelBase,
    IRecipient<BowlerHangCountChangedMessage>,
    IRecipient<PersonAddedOrChangedMessage>,
    IRecipient<PersonDeletedMessage>,
    IRecipient<SystemResetMessage>
{
    private readonly IPersonService _personService;
    private readonly INavigationService _navigationService;
    private readonly IWeekService _weekService;

    public BowlerOverviewViewModel(
        IPersonService personService,
        INavigationService navigationService,
        IWeekService weekService)
    {
        _personService = personService;
        _navigationService = navigationService;
        _weekService = weekService;

        WeakReferenceMessenger.Default.Register<BowlerHangCountChangedMessage>(this);
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

    // TODO: Add filtering by Subs,Regulars
    // TODO: Add sort by most hangs,least hangs, name asc/desc

    [RelayCommand]
    private async Task NavigateToAddBowler() => await _navigationService.GoToAddBowler();

    [RelayCommand]
    private async Task NavigateToEditSelectedBowler()
    {
        if (SelectedBowler is not null)
        {
            await _navigationService.GoToEditBowler(SelectedBowler.Map());
            SelectedBowler = null;
        }
    }

    public override async Task LoadAsync()
    {
        if (Bowlers.Count == 0)
        {
            await Loading(GetBowlers);
        }
    }

    private async Task GetBowlers()
    {
        var people = await _personService.GetAllPeople();
        if (people.Any())
        {
            Bowlers.Clear();
            AllBowlers = BowlerListItemViewModelMapper.Map(people.OrderBy(b => b.Name));
            Bowlers = AllBowlers.ToObservableCollection();

            await UpdateBowlerHangCounts();
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
                   .ForEach(bowler => bowler.Hangings = allWeeks.SelectMany(w => w.Bowlers.Where(b => (b.Status == Enums.Status.UsingSub ? b.SubId : b.PersonId) == bowler.Id))
                                                                .Sum(w => w.HangCount));

            Bowlers.Where(b => !b.IsSub).ToList().ForEach(b =>
            {
                b.HasLowestHangs = b.Hangings == Bowlers.Where(bw => !bw.IsSub).Min(bw => bw.Hangings);
            });
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

    public async void Receive(BowlerHangCountChangedMessage message) => await UpdateBowlerHangCounts();

    public async void Receive(PersonAddedOrChangedMessage message) => await GetBowlers();

    public async void Receive(PersonDeletedMessage message) => await GetBowlers();
}
