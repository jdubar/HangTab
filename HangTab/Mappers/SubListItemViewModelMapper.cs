using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public static class SubListItemViewModelMapper
{
    public static IEnumerable<SubListItemViewModel> Map(this IEnumerable<Person> people)
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
