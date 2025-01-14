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

    internal static List<BowlerListItemViewModel> MapBowlerToBowlerListItemViewModel(this IEnumerable<Bowler> bowlers)
    {
        return bowlers.Select(b => b.Map()).ToList();
    }

    internal static List<BowlerListItemViewModel> MapWeeklyLineupToBowlerListItemViewModel(this IEnumerable<WeeklyLineup> weeklyLineup)
    {
        return weeklyLineup.Select(wl => wl.Map()).ToList();
    }

    internal static List<WeeklyLineup> MapBowlerToWeeklyLineup(this IEnumerable<Bowler> bowlers)
    {
        return bowlers.Select(b => new WeeklyLineup
        {
            BowlerId = b.Id,
            Bowler = b,
            Status = Enums.BowlerStatus.Active,
            HangCount = 0,
        }).ToList();
    }

    private static BowlerListItemViewModel Map(this WeeklyLineup weeklyLineup)
    {
        return new BowlerListItemViewModel(
            weeklyLineup.BowlerId,
            weeklyLineup.Bowler.Name,
            weeklyLineup.Bowler.IsSub,
            weeklyLineup.HangCount,
            weeklyLineup.Bowler.ImageUrl,
            weeklyLineup.Status);
    }

    private static BowlerListItemViewModel Map(this Bowler bowler)
    {
        return new BowlerListItemViewModel(
            bowler.Id,
            bowler.Name,
            bowler.IsSub,
            default,
            bowler.ImageUrl);
    }
}
