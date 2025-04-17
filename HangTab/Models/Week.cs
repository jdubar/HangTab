using SQLite;

using SQLiteNetExtensions.Attributes;

namespace HangTab.Models;
[Table("Weeks")]
public class Week
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Number { get; set; }
    public int BusRides  { get; set; }

    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<Bowler> Bowlers { get; set; } = [];
}
