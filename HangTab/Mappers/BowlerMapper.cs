using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;

public class BowlerMapper : IMapper<CurrentWeekListItemViewModel, Bowler>
{
    public Bowler Map(CurrentWeekListItemViewModel vm)
    {
        return vm is null
            ? throw new ArgumentNullException(nameof(vm))
            : new Bowler
                {
                    Id = vm.BowlerId,
                    PersonId = vm.PersonId,
                    Status = vm.Status,
                    HangCount = vm.HangCount,
                    WeekId = vm.WeekId,
                    SubId = vm.SubId
                };
    }
}
