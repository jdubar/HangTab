using HangTab.Models;
using HangTab.ViewModels.Items;

namespace HangTab.Mappers;
public class WeekListItemViewModelMapper(
    IMapper<IEnumerable<Bowler>, IEnumerable<BowlerListItemViewModel>> mapper) : IMapper<IEnumerable<Week>, IEnumerable<WeekListItemViewModel>>
{
    public IEnumerable<WeekListItemViewModel> Map(IEnumerable<Week> weeks)
    {
        return weeks is null
            ? throw new ArgumentNullException(nameof(weeks))
            : weeks.Select(w => new WeekListItemViewModel(
                w.Id,
                w.Number,
                w.BusRides,
                w.Bowlers.Sum(b => b.HangCount),
                mapper.Map(w.Bowlers)));
    }
}
