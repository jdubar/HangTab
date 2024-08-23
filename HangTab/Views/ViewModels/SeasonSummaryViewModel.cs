using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
public partial class SeasonSummaryViewModel(IDatabaseService data) : BaseViewModel
{
    public ObservableRangeCollection<Bowler> LowestHangBowlers { get; set; } = [];
    public ObservableRangeCollection<Bowler> AllOtherBowlers { get; set; } = [];

    [RelayCommand]
    public async Task InitializeDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            var bowlers = await data.GetAllBowlers();
            if (bowlers is null)
            {
                return;
            }

            var lowestHangBowlers = bowlers.Where(b => !b.IsSub
                                                       && b.TotalHangings == bowlers.Where(b => !b.IsSub
                                                                                                && !b.IsHidden)
                                                                                    .Min(b => b.TotalHangings))
                                           .Take(3);

            var otherBowlers = bowlers.Except(lowestHangBowlers).OrderBy(b => b.IsSub).ThenBy(b => b.TotalHangings);

            AllOtherBowlers.Clear();
            AllOtherBowlers.AddRange(otherBowlers);

            LowestHangBowlers.Clear();
            LowestHangBowlers.AddRange(lowestHangBowlers);
        }, "");
    }
}
