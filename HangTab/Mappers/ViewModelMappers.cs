using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public static class ViewModelMappers
{
    public static Person ToPerson(this BowlerListItemViewModel vm)
    {
        return vm is null
            ? throw new ArgumentNullException(nameof(vm))
            : new Person
            {
                Id = vm.Id,
                Name = vm.Name,
                IsSub = vm.IsSub,
                ImageUrl = vm.ImageUrl,
            };
    }

    public static Bowler ToBowler(this CurrentWeekListItemViewModel vm)
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
