using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Services.Mappers;
internal static class BowlerMapper
{
    internal static Person MapBowlerListItemViewModelToPerson(this BowlerListItemViewModel bowlerListItemViewModel)
    {
        return new Person
        {
            Id = bowlerListItemViewModel.Id,
            Name = bowlerListItemViewModel.Name,
            IsSub = bowlerListItemViewModel.IsSub,
            ImageUrl = bowlerListItemViewModel.ImageUrl,
        };
    }

    internal static List<BowlerListItemViewModel> MapPersonToBowlerListItemViewModel(this IEnumerable<Person> people)
    {
        return people.Select(b => b.Map()).ToList();
    }

    internal static List<BowlerListItemViewModel> MapBowlerToBowlerListItemViewModel(this IEnumerable<Bowler> bowlers)
    {
        return bowlers.Select(wl => wl.MapBowlerToBowlerListItemViewModel()).ToList();
    }

    internal static List<Bowler> MapPeopleToBowlers(this IEnumerable<Person> people)
    {
        return people.Select(b => new Bowler
        {
            PersonId = b.Id,
            Person = b,
            Status = Enums.Status.Active,
            HangCount = 0,
        }).ToList();
    }

    internal static Bowler MapCurrentWeekListItemViewModelToBowler(this CurrentWeekListItemViewModel vm)
    {
        return new Bowler
        {
            Id = vm.BowlerId,
            PersonId = vm.PersonId,
            Status = vm.Status,
            HangCount = vm.HangCount,
        };
    }

    private static BowlerListItemViewModel MapBowlerToBowlerListItemViewModel(this Bowler bowler)
    {
        return new BowlerListItemViewModel(
            bowler.PersonId,
            bowler.Person.Name,
            bowler.Person.IsSub,
            bowler.HangCount,
            bowler.Id,
            bowler.Person.ImageUrl,
            bowler.Status);
    }

    private static BowlerListItemViewModel Map(this Person person)
    {
        return new BowlerListItemViewModel(
            person.Id,
            person.Name,
            person.IsSub,
            default,
            default,
            person.ImageUrl);
    }
}
