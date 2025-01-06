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

    [ObservableProperty]
    private ObservableCollection<BowlerGroup> _bowlers = [];
    //private ObservableCollection<BowlerListItemViewModel> _bowlers = [];

    [ObservableProperty]
    private BowlerListItemViewModel? _selectedBowler;

    [ObservableProperty]
    private string _searchText = string.Empty;

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
        //AllBowlers = await _bowlerService.GetBowlers();
        AllBowlers =
        [
            new() { Id = 1, FirstName = "Player", LastName = "One" },
            new() { Id = 2, FirstName = "Player", LastName = "Two" },
            new() { Id = 3, FirstName = "Player", LastName = "Three" },
            new() { Id = 4, FirstName = "Sub", LastName = "One", IsSub = true },
            new() { Id = 5, FirstName = "Sub", LastName = "Two", IsSub = true },
        ];
        var bowlersList = new List<BowlerGroup>
        {
            new("Regulars", AllBowlers.Where(b => !b.IsSub).Map()),
            new("Subs", AllBowlers.Where(b => b.IsSub).Map())
        };
        Bowlers.Clear();
        Bowlers = bowlersList.ToObservableCollection();
        // -------------------------------------------
        // Original code before groups were added
        // -------------------------------------------
        //var bowlers = await _bowlerService.GetBowlers();
        //List<BowlerListItemViewModel> listItems = [];
        //foreach (var bowler in bowlers)
        //{
        //    listItems.Add(bowler.Map());
        //}

        //Bowlers.Clear();
        //Bowlers = listItems.ToObservableCollection();
        // -------------------------------------------
    }

    [RelayCommand]
    private async Task SearchBowlers(string searchText)
    {
        if (string.IsNullOrEmpty(searchText))
        {
            await GetBowlers();
        }
        else
        {
            var regs = AllBowlers.Where(b => !b.IsSub && b.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).Map();
            var bowlersList = new List<BowlerGroup>
            {
                new("Regulars", regs),
                new("Subs", AllBowlers.Where(b => b.IsSub && b.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).Map())
            };

            Bowlers.Clear();
            Bowlers = bowlersList.ToObservableCollection();
        }
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
