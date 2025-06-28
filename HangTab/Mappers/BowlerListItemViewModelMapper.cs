using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public class BowlerListItemViewModelMapper : IMapper<IEnumerable<Person>, IEnumerable<BowlerListItemViewModel>>,
                                             IMapper<IEnumerable<Bowler>, IEnumerable<BowlerListItemViewModel>>
{
    public IEnumerable<BowlerListItemViewModel> Map(IEnumerable<Person> people)
    {
        return people is null
            ? throw new ArgumentNullException(nameof(people))
            : people.Select(Map);
    }

    public IEnumerable<BowlerListItemViewModel> Map(IEnumerable<Bowler> bowlers)
    {
        return bowlers is null
            ? throw new ArgumentNullException(nameof(bowlers))
            : bowlers.Select(Map);
    }

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
