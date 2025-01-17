using SQLite;

using SQLiteNetExtensions.Attributes;

namespace HangTab.Models;
[Table("Weeks")]
public class Week
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int WeekNumber { get; set; }
    public int BusRides  { get; set; }

    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<WeeklyLineup> Bowlers { get; set; } = [];
}
