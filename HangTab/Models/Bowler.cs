using HangTab.Enums;

using SQLite;

using SQLiteNetExtensions.Attributes;

namespace HangTab.Models;
[Table("Bowlers")]
public class Bowler
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public Status Status { get; set; } = Status.Active;
    public int HangCount { get; set; }

    [ForeignKey(typeof(Week))]
    public int WeekId { get; set; }

    [ForeignKey(typeof(Person))]
    public int PersonId { get; set; }

    public int? SubId { get; set; } = null;

    [ManyToOne(CascadeOperations = CascadeOperation.All)]
    public Person Person { get; set; } = new();
}
