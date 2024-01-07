using SQLite;

namespace HangTab.Models;

[Table("week")]
public class Week
{
    [PrimaryKey]
    public int Id { get; set; }
    public int WeekNumber { get; set; } = 1;
    public IEnumerable<Bowler> Bowlers { get; set; }
    public int BusRides { get; set; }
    public int BowlerIdWithLowestHangs { get; set; }

    public int GetBowlerIdWithLowestHangs() =>
        Bowlers.First(b => !b.IsSub && b.TotalHangings == Bowlers.Where(b => !b.IsSub).Min(b => b.TotalHangings)).Id;
}
