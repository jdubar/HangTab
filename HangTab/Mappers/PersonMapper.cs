using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;

public static class PersonMapper
{
    public static Person Map(this BowlerListItemViewModel bowlerListItemViewModel)
    {
        return new Person
        {
            Id = bowlerListItemViewModel.Id,
            Name = bowlerListItemViewModel.Name,
            IsSub = bowlerListItemViewModel.IsSub,
            ImageUrl = bowlerListItemViewModel.ImageUrl,
        };
    }
}
