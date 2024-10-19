using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Data;
using HangTab.Extensions;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public partial class SeasonSummaryViewModel(IAudioService audio,
                                            IDatabaseService data) : BaseViewModel
{
    public ObservableRangeCollection<Bowler> LowestHangBowlers { get; set; } = [];
    public ObservableRangeCollection<Bowler> AllOtherBowlers { get; set; } = [];

    [ObservableProperty]
    private int _busRideTotal;

    private IReadOnlyCollection<Bowler> _bowlers;

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        _bowlers = await data.GetAllBowlers();
        if (_bowlers is null)
        {
            return;
        }

        BusRideTotal = await data.GetBusRideTotal();

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
