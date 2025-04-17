using HangTab.Extensions;
using HangTab.Models;
using HangTab.ViewModels;

namespace HangTab.Services.Mappers;
internal static class WeekMapper
{
    internal static CurrentWeekListItemViewModel MapWeeklyLineupToCurrentWeekListItemViewModel(this WeeklyLineup wl)
    {
        return new CurrentWeekListItemViewModel(
            wl.WeekId,
            wl.Id,
            wl.BowlerId,
            wl.Status,
            wl.HangCount,
            wl.Bowler.Name,
            wl.Bowler.ImageUrl ?? string.Empty,
            wl.Bowler.IsSub,
            wl.Bowler.Name.GetInitials());
    }

    internal static WeeklyLineup MapCurrentWeekListItemViewModelToWeeklyLineup(this CurrentWeekListItemViewModel cw)
    {
        return new WeeklyLineup
        {
            Id = cw.WeeklyLineupId,
            WeekId = cw.WeekId,
            BowlerId = cw.BowlerId,
            Status = cw.Status,
            HangCount = cw.HangCount,
            Bowler = new Bowler
            {
                Id = cw.BowlerId,
                Name = cw.Name,
                ImageUrl = cw.ImageUrl,
                IsSub = cw.IsSub,
                IsInactive = false,
            },
        };
    }

    internal static Week MapCurrentWeekOverviewViewmodelToWeek(this CurrentWeekOverviewViewModel cw)
    {
        return new Week
        {
            Id = cw.Id,
            WeekNumber = cw.WeekNumber,
            BusRides = cw.BusRides,
        };
    }

    internal static List<CurrentWeekListItemViewModel> MapBowlerToBowlerListItemViewModel(this IEnumerable<WeeklyLineup> bowlers)
    {
        return bowlers.Select(b => b.MapWeeklyLineupToCurrentWeekListItemViewModel()).ToList();
    }
}
