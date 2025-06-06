using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Services.Mappers;
internal static class PersonMapper
{
    internal static List<SubListItemViewModel> MapPersonToSubListItemViewModel(this IEnumerable<Person> people)
    {
        return people.Select(b => new SubListItemViewModel(
            b.Id,
            b.Name,
            b.IsSub,
            b.ImageUrl)).ToList();
    }
}
