using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public static class PersonMappers
{
    public static IEnumerable<BowlerListItemViewModel> ToBowlerListItemViewModelList(this IEnumerable<Person> people)
    {
        return people is null
            ? throw new ArgumentNullException(nameof(people))
            : people.Select(ToBowlerListItemViewModel);
    }

    public static IEnumerable<SubListItemViewModel> ToSubListItemViewModelList(this IEnumerable<Person> people)
    {
        return people is null
            ? throw new ArgumentNullException(nameof(people))
            : people.Select(ToSubListItemViewModel);
    }

    private static SubListItemViewModel ToSubListItemViewModel(Person person)
    {
        return person is null
            ? throw new ArgumentNullException(nameof(person))
            : new SubListItemViewModel
            {
                Id = person.Id,
                Name = person.Name,
                IsSub = person.IsSub,
                IsDeleted = person.IsDeleted,
                ImageUrl = person.ImageUrl
            };
    }

    private static BowlerListItemViewModel ToBowlerListItemViewModel(Person person)
    {
        return person is null
            ? throw new ArgumentNullException(nameof(person))
            : new BowlerListItemViewModel
            {
                Id = person.Id,
                Name = person.Name,
                IsSub = person.IsSub,
                IsDeleted = person.IsDeleted,
                ImageUrl = person.ImageUrl
            };
    }
}
