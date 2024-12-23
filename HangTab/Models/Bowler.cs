using SQLite;
using SQLiteNetExtensions.Attributes;

namespace HangTab.Models;
[Table("Bowlers")]
public class Bowler
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsSub { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
