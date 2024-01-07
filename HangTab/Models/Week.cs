using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace HangTab.Models;

[Table("week")]
public class Week : ObservableObject
{
    private int _busRides;

    [PrimaryKey, AutoIncrement]
    public int WeekNumber { get; set; } = 1;
    public IEnumerable<Bowler> Bowlers { get; set; }
    public int BusRides
    {
        get => _busRides;
        set => SetProperty(ref _busRides, value);
    }

    public int GetBowlerIdWithLowestHangs() =>
        Bowlers.First(b => !b.IsSub && b.TotalHangings == Bowlers.Where(b => !b.IsSub).Min(b => b.TotalHangings)).Id;
}
