using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Extensions;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public partial class SeasonSummaryViewModel(IAudioService audio,
                                            IBowlerService bowlerService,
                                            IBusRideService busRideService) : BaseViewModel
{
    public ObservableRangeCollection<Bowler> LowestHangBowlers { get; set; } = [];
    public ObservableRangeCollection<Bowler> AllOtherBowlers { get; set; } = [];

    [ObservableProperty]
    private int _busRideTotal;

    private IReadOnlyCollection<Bowler> _bowlers;

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        _bowlers = await bowlerService.GetAll();
        if (_bowlers is null)
        {
            return;
        }

        BusRideTotal = await busRideService.GetTotal();

        SetBowlerLists();
    }

    [RelayCommand]
    private void PlayBusSound() => audio.PlayBusRideSound();

    private void SetBowlerLists()
    {
        var lowestHangBowlers = _bowlers.GetLowestHangBowlers().Take(3).ToList();
        LowestHangBowlers.AddBowlersToCollection(lowestHangBowlers);
        var otherBowlers = _bowlers.Except(lowestHangBowlers).OrderBy(b => b.IsSub).ThenBy(b => b.TotalHangings);
        AllOtherBowlers.AddBowlersToCollection(otherBowlers);
    }
}
