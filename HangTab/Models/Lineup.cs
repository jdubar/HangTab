using HangTab.Enums;
using HangTab.Models.Wrappers;

using SQLite;

using SQLiteNetExtensions.Attributes;

namespace HangTab.Models;
public class Lineup
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; private set; }
    public Bowler Bowler { get; set; }
    public BowlerStatus Status { get; set; }
    public int HangCount { get; set; }

    [ForeignKey(typeof(Week), Name = "Id")]
    public int WeekId { get; set; }

    public static Lineup GenerateNewFromWrapper(LineupWrapper wrapper)
    {
        return new Lineup
        {
            Id = wrapper.Id,
            Bowler = wrapper.Bowler,
            Status = wrapper.Status,
            HangCount = wrapper.HangCount,
            WeekId = wrapper.WeekId,
        };
    }
}
