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
    private string _searchText;

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
        var allBowlers = await _bowlerService.GetAllBowlers();
        if (allBowlers.Any())
        {
            Bowlers.Clear();
            AllBowlers = allBowlers.OrderBy(b => b.Name).MapBowlerToBowlerListItemViewModel();
            Bowlers = AllBowlers.ToObservableCollection();
        }
    }

    private async Task UpdateBowlerHangCounts()
    {
        if (Bowlers.Count > 0)
        {
            var allWeeks = await _weekService.GetAllWeeks();
            Bowlers.ToList().ForEach(b => b.Hangings = allWeeks.SelectMany(w => w.Bowlers.Where(wl => wl.BowlerId == b.Id)).Sum(w => w.HangCount));
        }
    }

    private Task SearchBowlers(string searchText)
    {
        Bowlers.Clear();
        Bowlers = AllBowlers.Where(b => b.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToObservableCollection();
        return Task.CompletedTask;
    }

    public async void Receive(BowlerAddedOrChangedMessage message) => await UpdateBowlerList();

    public async void Receive(BowlerDeletedMessage message) => await UpdateBowlerList();

    private async Task UpdateBowlerList()
    {
        Bowlers.Clear();
        await GetBowlers();
    }
}
