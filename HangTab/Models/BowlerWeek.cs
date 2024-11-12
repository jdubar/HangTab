using HangTab.Models.Wrappers;

using SQLite;
using SQLiteNetExtensions.Attributes;

namespace HangTab.Models;

public class BowlerWeek
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; private set; }
    public int WeekNumber { get; set; }
    public int Hangings  { get; set; }

    [ForeignKey(typeof(Bowler), Name = "Id")]
    public int BowlerId { get; set; }

    public static BowlerWeek GenerateNewFromWrapper(BowlerWeekWrapper wrapper)
    {
        return new BowlerWeek
        {
            Id = wrapper.Id,
            WeekNumber = wrapper.WeekNumber,
            Hangings = wrapper.Hangings,
            BowlerId = wrapper.BowlerId,
        };
    }
}
