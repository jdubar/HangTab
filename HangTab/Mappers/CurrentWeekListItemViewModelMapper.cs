using HangTab.Extensions;
using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public class CurrentWeekListItemViewModelMapper : IMapper<IEnumerable<Bowler>, IEnumerable<CurrentWeekListItemViewModel>>
{
    public IEnumerable<CurrentWeekListItemViewModel> Map(IEnumerable<Bowler> bowlers)
    {
        return bowlers is null
            ? throw new ArgumentNullException(nameof(bowlers))
            : bowlers.Select(Map);
    }

    private static CurrentWeekListItemViewModel Map(Bowler bowler)
    {
        return bowler is null
            ? throw new ArgumentNullException(nameof(bowler))
            : new CurrentWeekListItemViewModel(
                bowler.WeekId,
                bowler.Id,
                bowler.PersonId,
                bowler.SubId,
                bowler.Status,
                bowler.HangCount,
                bowler.Person.Name,
                bowler.Person.IsSub,
                bowler.Person.Name.GetInitials(),
                bowler.Person.ImageUrl);
    }
}
