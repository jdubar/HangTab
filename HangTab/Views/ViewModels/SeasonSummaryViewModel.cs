using CommunityToolkit.Mvvm.Input;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
public partial class SeasonSummaryViewModel(IDatabaseService data) : BaseViewModel
{
    public ObservableRangeCollection<Bowler> LowestHangBowlers { get; set; } = [];
    public ObservableRangeCollection<Bowler> AllOtherBowlers { get; set; } = [];

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            var bowlers = await data.GetAllBowlers();
            if (bowlers is null)
            {
                return;
            }

            var lowestHangBowlers = bowlers.Where(b => !b.IsSub
                                                       && b.TotalHangings == bowlers.Where(bowler => !bowler.IsSub
                                                                                                && !bowler.IsHidden)
                                                                                    .Min(bowler => bowler.TotalHangings))
                                           .Take(3)
                                           .ToList();

            var otherBowlers = bowlers.Except(lowestHangBowlers).OrderBy(b => b.IsSub).ThenBy(b => b.TotalHangings);

            AllOtherBowlers.Clear();
            AllOtherBowlers.AddRange(otherBowlers);

            LowestHangBowlers.Clear();
            LowestHangBowlers.AddRange(lowestHangBowlers);
        }, "");
    }
}
