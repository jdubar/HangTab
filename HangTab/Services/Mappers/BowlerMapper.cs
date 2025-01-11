using HangTab.Models;
using HangTab.ViewModels;

namespace HangTab.Services.Mappers;
internal static class BowlerMapper
{
    internal static Bowler Map(this BowlerListItemViewModel bowlerListItemViewModel)
    {
        return new Bowler
        {
            Id = bowlerListItemViewModel.Id,
            Name = bowlerListItemViewModel.Name,
            IsSub = bowlerListItemViewModel.IsSub,
            ImageUrl = bowlerListItemViewModel.ImageUrl,
        };
    }

    internal static List<BowlerListItemViewModel> Map(this IEnumerable<Bowler> bowlers)
    {
        return bowlers.Select(b => b.Map()).ToList();
    }

    internal static List<BowlerListItemViewModel> Map(this IEnumerable<WeeklyLineup> bowlers)
    {
        return bowlers.Select(b => new BowlerListItemViewModel(
            b.BowlerId,
            b.Bowler.Name,
            b.Bowler.IsSub,
            b.HangCount,
            b.Position,
            b.Bowler.ImageUrl,
            b.Status)).ToList();
    }

    private static BowlerListItemViewModel Map(this Bowler bowler)
    {
        return new BowlerListItemViewModel(
            bowler.Id,
            bowler.Name,
            bowler.IsSub,
            default,
            default,
            bowler.ImageUrl);
    }
}
