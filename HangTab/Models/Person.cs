using SQLite;

namespace HangTab.Models;
[Table("People")]
public class Person
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsInactive { get; set; }
    public bool IsSub { get; set; }
}
