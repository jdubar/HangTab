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

    public BowlerListOverviewViewModel(IBowlerService bowlerService, INavigationService navigationService)
    {
        _bowlerService = bowlerService;
        _navigationService = navigationService;

        WeakReferenceMessenger.Default.Register<BowlerAddedOrChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<BowlerDeletedMessage>(this);
    }

    private IEnumerable<Bowler> AllBowlers { get; set; } = [];
    private List<BowlerGroup> _allBowlersInGroups = [];

    [ObservableProperty]
    private ObservableCollection<BowlerGroup> _bowlers = [];

    [ObservableProperty]
    private BowlerListItemViewModel? _selectedBowler;

    [ObservableProperty]
    private string _searchText;

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Bowlers.Clear();
            Bowlers = _allBowlersInGroups.ToObservableCollection();
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
        if (Bowlers.Count == 0)
        {
            await Loading(GetBowlers);
        }
    }

    private async Task GetBowlers()
    {
        AllBowlers = await _bowlerService.GetBowlers();
        // TODO: Remove this when the service is implemented
        AllBowlers =
        [
            new Bowler { Id = 1, Name = "Player One" },
            new Bowler { Id = 2, Name = "Player Two" },
            new Bowler { Id = 3, Name = "Player Three" },
            new Bowler { Id = 4, Name = "Sub One", IsSub = true },
            new Bowler { Id = 5, Name = "Sub Two", IsSub = true },
        ];
        _allBowlersInGroups =
        [
            new BowlerGroup("Regulars", AllBowlers.Where(b => !b.IsSub).Map()),
            new BowlerGroup("Subs", AllBowlers.Where(b => b.IsSub).Map())
        ];
        Bowlers.Clear();
        Bowlers = _allBowlersInGroups.ToObservableCollection();
    }

    private Task SearchBowlers(string searchText)
    {
        var filteredBowlerGroups = new List<BowlerGroup>
        {
            new("Regulars", AllBowlers.Where(b => !b.IsSub && b.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).Map()),
            new("Subs", AllBowlers.Where(b => b.IsSub && b.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).Map())
        };

        Bowlers.Clear();
        Bowlers = filteredBowlerGroups.ToObservableCollection();
        return Task.CompletedTask;
    }

    public async void Receive(BowlerAddedOrChangedMessage message)
    {
        try
        {
            Bowlers.Clear();
            await GetBowlers();
        }
        catch (Exception)
        {
            // TODO: handle exception
        }
    }

    public void Receive(BowlerDeletedMessage message)
    {
        ////var deletedEvent = Bowlers.FirstOrDefault(e => e.Id == message.Id);
        //var deletedEvent = Bowlers.SelectMany(g => g..Bowlers).FirstOrDefault(e => e.Id == message.Id);

        //if (deletedEvent is not null)
        //{
        //    //Bowlers.Remove(deletedEvent);
        //    Bowlers.SelectMany(g => g.Bowlers).ToObservableCollection().Remove(deletedEvent);
        //}
    }
}
