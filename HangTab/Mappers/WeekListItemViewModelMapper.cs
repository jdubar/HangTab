using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public static class WeekListItemViewModelMapper
{
    public static  IEnumerable<WeekListItemViewModel> Map(this IEnumerable<Week> weeks)
        => weeks.Select(w => new WeekListItemViewModel(
            w.Id,
            w.Number,
            w.BusRides,
            w.Bowlers.Sum(b => b.HangCount),
            BowlerListItemViewModelMapper.Map(w.Bowlers)));
}
