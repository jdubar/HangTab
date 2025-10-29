using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public static class WeekMappers
{
    public static IEnumerable<WeekListItemViewModel> ToWeekListItemViewModelList(this IEnumerable<Week> weeks)
    {
        return weeks is null
            ? throw new ArgumentNullException(nameof(weeks))
            : weeks.Select(ToWeekListItemViewModel);
    }

    private static WeekListItemViewModel ToWeekListItemViewModel(Week week)
    {
        return week is null
            ? throw new ArgumentNullException(nameof(week))
            : new WeekListItemViewModel
            {
                Id = week.Id,
                Number = week.Number,
                BusRides = week.BusRides,
                TotalHangCount = week.Bowlers.Sum(b => b.HangCount)
            };
    }
}
