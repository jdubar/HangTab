using HangTab.Models;
using HangTab.ViewModels;

namespace HangTab.Services.Mappers;
internal static class BowlerMapper
{
    internal static Bowler MapBowlerListItemViewModelToBowler(this BowlerListItemViewModel bowlerListItemViewModel)
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
        return weeklyLineup.Select(wl => wl.MapWeeklyLineupToBowlerListItemViewModel()).ToList();
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

    internal static WeeklyLineup MapCurrentWeekListItemViewModelToBowler(this CurrentWeekListItemViewModel vm)
    {
        return new WeeklyLineup
        {
            Id = vm.WeeklyLineupId,
            BowlerId = vm.BowlerId,
            Status = vm.Status,
            HangCount = vm.HangCount,
        };
    }

    private static BowlerListItemViewModel MapWeeklyLineupToBowlerListItemViewModel(this WeeklyLineup weeklyLineup)
    {
        return new BowlerListItemViewModel(
            weeklyLineup.BowlerId,
            weeklyLineup.Bowler.Name,
            weeklyLineup.Bowler.IsSub,
            weeklyLineup.HangCount,
            weeklyLineup.Id,
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
            default,
            bowler.ImageUrl);
    }
}
