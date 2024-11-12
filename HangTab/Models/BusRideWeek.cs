using SQLite;

namespace HangTab.Models;

public class BusRideWeek
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; private init; }
    public int WeekNumber { get; set; }
    public int BusRides { get; set; }
}
