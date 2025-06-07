using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public static class BowlerListItemViewModelMapper
{
    public static IEnumerable<BowlerListItemViewModel> Map(this IEnumerable<Person> people) => people.Select(Map);

    public static IEnumerable<BowlerListItemViewModel> Map(this IEnumerable<Bowler> bowlers) => bowlers.Select(Map);

    private static BowlerListItemViewModel Map(Bowler bowler)
    {
        return bowler is null
            ? throw new ArgumentNullException(nameof(bowler))
            : new BowlerListItemViewModel(
                bowler.PersonId,
                bowler.Person.Name,
                bowler.Person.IsSub,
                bowler.Id,
                bowler.HangCount,
                bowler.Person.ImageUrl,
                bowler.Status);
    }

    private static BowlerListItemViewModel Map(Person person)
    {
        return person is null
            ? throw new ArgumentNullException(nameof(person))
            : new BowlerListItemViewModel(
                person.Id,
                person.Name,
                person.IsSub,
                default,
                default,
                person.ImageUrl);
    }
}
