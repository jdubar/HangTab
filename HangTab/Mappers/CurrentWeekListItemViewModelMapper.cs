using HangTab.Extensions;
using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public static class CurrentWeekListItemViewModelMapper
{
    public static IEnumerable<CurrentWeekListItemViewModel> Map(this IEnumerable<Bowler> bowlers)
    {
        return bowlers.Select(Map);
    }

    private static CurrentWeekListItemViewModel Map(Bowler bowler)
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
}
