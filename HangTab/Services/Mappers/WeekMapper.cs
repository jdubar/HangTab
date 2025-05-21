using HangTab.Extensions;
using HangTab.Models;
using HangTab.ViewModels;

namespace HangTab.Services.Mappers;
internal static class WeekMapper
{
    internal static CurrentWeekListItemViewModel MapBowlerToCurrentWeekListItemViewModel(this Bowler wl)
    {
        return new CurrentWeekListItemViewModel(
            wl.WeekId,
            wl.Id,
            wl.PersonId,
            wl.Status,
            wl.HangCount,
            wl.Person.Name,
            wl.Person.IsSub,
            wl.Person.Name.GetInitials() ?? string.Empty, // Ensure initials are not null
            wl.Person.ImageUrl);
    }

    internal static Bowler MapCurrentWeekListItemViewModelToBowler(this CurrentWeekListItemViewModel cw)
    {
        return new Bowler
        {
            Id = cw.BowlerId,
            WeekId = cw.WeekId,
            PersonId = cw.PersonId,
            Status = cw.Status,
            HangCount = cw.HangCount,
            Person = new Person
            {
                Id = cw.BowlerId,
                Name = cw.Name,
                ImageUrl = cw.ImageUrl,
                IsSub = cw.IsSub,
            },
        };
    }

    internal static Week MapCurrentWeekOverviewViewmodelToWeek(this CurrentWeekOverviewViewModel cw)
    {
        return new Week
        {
            Id = cw.Id,
            Number = cw.WeekNumber,
            BusRides = cw.BusRides,
        };
    }

    internal static List<CurrentWeekListItemViewModel> MapBowlerToBowlerListItemViewModel(this IEnumerable<Bowler> bowlers)
    {
        return bowlers.Select(b => b.MapBowlerToCurrentWeekListItemViewModel()).ToList();
    }
}
