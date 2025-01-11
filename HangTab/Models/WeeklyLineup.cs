using HangTab.Enums;

using SQLite;

using SQLiteNetExtensions.Attributes;

namespace HangTab.Models;
[Table("WeeklyLineups")]
public class WeeklyLineup
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Position { get; set; }
    public BowlerStatus Status { get; set; } = BowlerStatus.Active;
    public int HangCount { get; set; }

    [ForeignKey(typeof(Week))]
    public int WeekId { get; set; }

    [ForeignKey(typeof(Bowler))]
    public int BowlerId { get; set; }

    [OneToOne]
    public Bowler Bowler { get; set; } = new();
}
