using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
public partial class SeasonSummaryViewModel(IDatabaseService data) : BaseViewModel
{
    public ObservableRangeCollection<Bowler> LowestHangBowlers { get; set; } = [];
    public ObservableRangeCollection<Bowler> AllBowlers { get; set; } = [];

    [RelayCommand]
    public async Task InitializeDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            var bowlers = await data.GetAllBowlers();
            var lowestHangBowlers = bowlers.Where(b => !b.IsSub
                                                       && b.TotalHangings == bowlers.Where(b => !b.IsSub).Min(b => b.TotalHangings));

            AllBowlers.Clear();
            AllBowlers.AddRange(bowlers);

            LowestHangBowlers.Clear();
            LowestHangBowlers.AddRange(lowestHangBowlers);
        }, "");
    }
}
