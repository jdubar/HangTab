using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;

public class PersonMapper : IMapper<BowlerListItemViewModel, Person>
{
    public Person Map(BowlerListItemViewModel vm)
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
}
