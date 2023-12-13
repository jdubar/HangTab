using SQLite;

namespace HangTab.Models;

[Table("week")]
public class Week
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int WeekNumber { get; set; }
    public int BowlerId { get; set; }
    public int Hangings { get; set; }
}
