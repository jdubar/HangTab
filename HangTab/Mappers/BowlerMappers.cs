using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public static class BowlerMappers
{
    public static IEnumerable<CurrentWeekListItemViewModel> ToCurrentWeekListItemViewModelList(this IEnumerable<Bowler> bowlers)
    {
        return bowlers is null
            ? throw new ArgumentNullException(nameof(bowlers))
            : bowlers.Select(ToCurrentWeekListItemViewModel);
    }

    public static IEnumerable<BowlerListItemViewModel> ToBowlerListItemViewModelList(this IEnumerable<Bowler> bowlers)
    {
        return bowlers is null
            ? throw new ArgumentNullException(nameof(bowlers))
            : bowlers.Select(ToBowlerListItemViewModel);
    }

    private static BowlerListItemViewModel ToBowlerListItemViewModel(Bowler bowler)
    {
        return bowler is null
            ? throw new ArgumentNullException(nameof(bowler))
            : new BowlerListItemViewModel
            {
                Id = bowler.PersonId,
                Name = bowler.Person.Name,
                IsSub = bowler.Person.IsSub,
                IsDeleted = bowler.Person.IsDeleted,
                BowlerId = bowler.Id,
                HangCount = bowler.HangCount,
                ImageUrl = bowler.Person.ImageUrl,
                Status = bowler.Status
            };
    }

    private static CurrentWeekListItemViewModel ToCurrentWeekListItemViewModel(Bowler bowler)
    {
        return bowler is null
            ? throw new ArgumentNullException(nameof(bowler))
            : new CurrentWeekListItemViewModel
            {
                WeekId = bowler.WeekId,
                BowlerId = bowler.Id,
                PersonId = bowler.PersonId,
                SubId = bowler.SubId,
                Status = bowler.Status,
                HangCount = bowler.HangCount,
                Name = bowler.Person.Name,
                IsSub = bowler.Person.IsSub,
                ImageUrl = bowler.Person.ImageUrl
            };
    }
}
