using HangTab.Models;
using HangTab.ViewModels.Items;
using HangTab.ViewModels.Items.Interfaces;

namespace HangTab.Extensions;
public static class ListItemExtensions
{
    public static void SetLowestBowlerHangCount<T>(this IEnumerable<T> bowlers) where T : ILowestHangCountBowler
    {
        bowlers.Where(b => !b.IsSub).ToList().ForEach(b =>
        {
            b.HasLowestHangCount = b.HangCount == bowlers.Where(bw => !bw.IsSub).Min(bw => bw.HangCount);
        });
    }

    public static void SetBowlerHangSumByWeeks(this IEnumerable<BowlerListItemViewModel> bowlers, IEnumerable<Week> weeks)
    {
        bowlers.ToList().ForEach(bowler => bowler.HangCount = weeks.SelectMany(w => w.Bowlers.Where(b => (b.Status == Enums.Status.UsingSub ? b.SubId : b.PersonId) == bowler.Id))
                                                                   .Sum(w => w.HangCount));
    }
}
