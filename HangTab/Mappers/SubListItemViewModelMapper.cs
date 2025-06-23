using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public class SubListItemViewModelMapper : IMapper<IEnumerable<Person>, IEnumerable<SubListItemViewModel>>
{
    public IEnumerable<SubListItemViewModel> Map(IEnumerable<Person> people)
    {
        return people is null
            ? throw new ArgumentNullException(nameof(people))
            : people.Select(Map);
    }

    private static SubListItemViewModel Map(Person person)
    {
        return person is null
            ? throw new ArgumentNullException(nameof(person))
            : new SubListItemViewModel(
                person.Id,
                person.Name,
                person.IsSub,
                person.ImageUrl);
    }
}
