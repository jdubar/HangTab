using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

namespace HangTab.Models.Wrappers;
public partial class WeekWrapper : ObservableObject
{
    public WeekWrapper(Week week)
    {
        if (week is null)
        {
            return;
        }

        Id = week.Id;
        WeekNumber = week.WeekNumber;
        BusRideCount = week.BusRideCount;

        Lineup = [];
        week.Lineup?.ToList().ForEach(lineup => Lineup.Add(new LineupWrapper(lineup)));
    }

    public int Id { get; }
    public int WeekNumber { get; set; }

    public ObservableCollection<LineupWrapper> Lineup { get; set; }

    [ObservableProperty]
    private int _busRideCount;
}