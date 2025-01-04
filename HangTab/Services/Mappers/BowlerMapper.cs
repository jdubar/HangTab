using HangTab.Models;
using HangTab.ViewModels;

namespace HangTab.Services.Mappers;
internal static class BowlerMapper
{
    internal static Bowler Map(this BowlerListItemViewModel bowlerListItemViewModel)
    {
        return new Bowler
        {
            Id = bowlerListItemViewModel.Id,
            FirstName = bowlerListItemViewModel.FirstName,
            LastName = bowlerListItemViewModel.LastName ?? string.Empty,
            IsSub = bowlerListItemViewModel.IsSub,
            ImageUrl = bowlerListItemViewModel.ImageUrl ?? string.Empty,
        };
    }

    internal static BowlerListItemViewModel Map(this Bowler bowler)
    {
        return new BowlerListItemViewModel(
            bowler.Id,
            bowler.FirstName,
            bowler.IsSub,
            bowler.FullName,
            bowler.LastName,
            bowler.ImageUrl);
    }

    internal static List<BowlerListItemViewModel> Map(this IEnumerable<Bowler> bowlers)
    {
        return bowlers.Select(b => b.Map()).ToList();
    }
}
