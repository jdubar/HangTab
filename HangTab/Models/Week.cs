using HangTab.Models.Wrappers;

using SQLite;

using SQLiteNetExtensions.Attributes;

namespace HangTab.Models;

public class Week
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; private set; }
    public int WeekNumber { get; set; }
    public int BusRideCount  { get; set; }

    [OneToMany]
    public IEnumerable<Lineup> Lineup { get; set; }

    public static Week GenerateNewFromWrapper(WeekWrapper wrapper)
    {
        return new Week
        {
            Id = wrapper.Id,
            WeekNumber = wrapper.WeekNumber,
            BusRideCount = wrapper.BusRideCount,
        };
    }
}