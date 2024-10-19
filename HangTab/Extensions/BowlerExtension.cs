using MvvmHelpers;

namespace HangTab.Extensions;
public static class BowlerExtension
{
    public static List<Bowler> GetLowestHangBowlers(this IReadOnlyCollection<Bowler> bowlers)
    {
        return bowlers.Where(b => !b.IsSub
                                     && b.TotalHangings == bowlers.Where(bowler => !bowler.IsSub).Min(bowler => bowler.TotalHangings)).ToList();
    }

    public static void AddBowlersToCollection(this ObservableRangeCollection<Bowler> collection, IEnumerable<Bowler> bowlers)
    {
        collection.Clear();
        collection.AddRange(bowlers);
    }
}