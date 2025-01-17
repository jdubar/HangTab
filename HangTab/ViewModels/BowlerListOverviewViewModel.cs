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
public partial class BowlerListOverviewViewModel :
    ViewModelBase,
    IRecipient<BowlerAddedOrChangedMessage>,
    IRecipient<BowlerDeletedMessage>
{
    private readonly IBowlerService _bowlerService;
    private readonly INavigationService _navigationService;
    private readonly IWeekService _weekService;

    public BowlerListOverviewViewModel(
        IBowlerService bowlerService,
        INavigationService navigationService,
        IWeekService weekService)
    {
        _bowlerService = bowlerService;
        _navigationService = navigationService;
        _weekService = weekService;

        WeakReferenceMessenger.Default.Register<BowlerAddedOrChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<BowlerDeletedMessage>(this);
        _weekService = weekService;
    }

    private IEnumerable<Bowler> _allBowlers = [];
    private IEnumerable<Bowler> AllBowlers
    {
        get => _allBowlers;
        set
        {
            SetProperty(ref _allBowlers, value);
            if (!_allBowlers.Any())
            {
                Groups.Clear();
            }
        }
    }
    private List<BowlerGroup> _allBowlersInGroups = [];

    [ObservableProperty]
    private ObservableCollection<BowlerGroup> _groups = [];

    [ObservableProperty]
    private BowlerListItemViewModel? _selectedBowler;

    [ObservableProperty]
    private string _searchText;

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Groups.Clear();
            Groups = _allBowlersInGroups.ToObservableCollection();
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
            await _navigationService.GoToEditBowler(SelectedBowler.Map());
            SelectedBowler = null;
        }
    }

    public override async Task LoadAsync()
    {
        if (Groups.Count == 0)
        {
            await Loading(GetBowlers);
        }

        await UpdateBowlerHangCounts();
    }

    private async Task GetBowlers()
    {
        AllBowlers = await _bowlerService.GetAllBowlers();
        _allBowlersInGroups =
        [
            new BowlerGroup("Regulars", AllBowlers.Where(b => !b.IsSub).MapBowlerToBowlerListItemViewModel()),
            new BowlerGroup("Subs", AllBowlers.Where(b => b.IsSub).MapBowlerToBowlerListItemViewModel())
        ];

        Groups.Clear();
        Groups = _allBowlersInGroups.ToObservableCollection();
    }

    private async Task UpdateBowlerHangCounts()
    {
        var allWeeks = await _weekService.GetAllWeeks();
        foreach (var group in Groups)
        {
            group.ForEach(b => b.Hangings = allWeeks.SelectMany(w => w.Bowlers.Where(wl => wl.BowlerId == b.Id)).Sum(w => w.HangCount));
        }
    }

    private Task SearchBowlers(string searchText)
    {
        var filteredBowlerGroups = new List<BowlerGroup>
        {
            new("Regulars", AllBowlers.Where(b => !b.IsSub && b.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).MapBowlerToBowlerListItemViewModel()),
            new("Subs", AllBowlers.Where(b => b.IsSub && b.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).MapBowlerToBowlerListItemViewModel())
        };

        Groups.Clear();
        Groups = filteredBowlerGroups.ToObservableCollection();
        return Task.CompletedTask;
    }

    public async void Receive(BowlerAddedOrChangedMessage message)
    {
        Groups.Clear();
        await GetBowlers();
    }

    public void Receive(BowlerDeletedMessage message)
    {
        var deletedBowler = Groups.SelectMany(g => g).FirstOrDefault(b => b.Id == message.Id);
        if (deletedBowler is not null)
        {
            Groups.First(g => g.Contains(deletedBowler)).Remove(deletedBowler);
            AllBowlers = AllBowlers.Where(b => b.Id != message.Id);
        }
    }
}
