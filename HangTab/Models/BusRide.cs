using SQLite;

namespace HangTab.Models;

public class BusRide
{
    [PrimaryKey]
    public int Id { get; private init; } = 1;
    public int Total { get; set; }
}
