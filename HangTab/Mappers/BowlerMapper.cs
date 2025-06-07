using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;

public static class BowlerMapper
{
    public static Bowler Map(this CurrentWeekListItemViewModel vm)
    {
        return new Bowler
        {
            Id = vm.BowlerId,
            PersonId = vm.PersonId,
            Status = vm.Status,
            HangCount = vm.HangCount,
        };
    }
}
