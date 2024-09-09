using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Extensions;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
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
    private async Task PlayBusSound()
        => await audio.PlayBusSound();

    private void SetBowlerLists()
    {
        var lowestHangBowlers = _bowlers.GetLowestHangBowlers().Take(3).ToList();
        var otherBowlers = _bowlers.Except(lowestHangBowlers).OrderBy(b => b.IsSub).ThenBy(b => b.TotalHangings);
        AllOtherBowlers.Clear();
        AllOtherBowlers.AddRange(otherBowlers);

        LowestHangBowlers.Clear();
        LowestHangBowlers.AddRange(lowestHangBowlers);
    }
}
