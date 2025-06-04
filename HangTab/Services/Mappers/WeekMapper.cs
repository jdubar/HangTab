using HangTab.Extensions;
using HangTab.Models;
using HangTab.ViewModels;

namespace HangTab.Services.Mappers;
internal static class WeekMapper
{
    internal static CurrentWeekListItemViewModel MapBowlerToCurrentWeekListItemViewModel(this Bowler bowler)
    {
        return new CurrentWeekListItemViewModel(
            bowler.WeekId,
            bowler.Id,
            bowler.PersonId,
            bowler.SubId,
            bowler.Status,
            bowler.HangCount,
            bowler.Person.Name,
            bowler.Person.IsSub,
            bowler.Person.Name.GetInitials() ?? string.Empty, // Ensure initials are not null
            bowler.Person.ImageUrl);
    }

    internal static Bowler MapCurrentWeekListItemViewModelToBowler(this CurrentWeekListItemViewModel cw)
    {
        return new Bowler
        {
            Id = cw.BowlerId,
            WeekId = cw.WeekId,
            PersonId = cw.PersonId,
            Status = cw.Status,
            SubId = cw.SubId,
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

    internal static List<WeekListItemViewModel> MapWeekToWeekListItemViewModel(this IEnumerable<Week> weeks)
    {
        return weeks.Select(w => new WeekListItemViewModel
        {
            Id = w.Id,
            Number = w.Number,
            BusRides = w.BusRides,
        }).ToList();
    }
}
