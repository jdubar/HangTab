using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public class WeekListItemViewModelMapper : IMapper<IEnumerable<Week>, IEnumerable<WeekListItemViewModel>>
{
    public IEnumerable<WeekListItemViewModel> Map(IEnumerable<Week> weeks)
    {
        return weeks is null
            ? throw new ArgumentNullException(nameof(weeks))
            : weeks.Select(Map);
    }

    private static WeekListItemViewModel Map(Week week)
    {
        return week is null
            ? throw new ArgumentNullException(nameof(week))
            : new WeekListItemViewModel(
                week.Id,
                week.Number,
                week.BusRides,
                week.Bowlers.Sum(b => b.HangCount));
    }
}
