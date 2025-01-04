using SQLite;

namespace HangTab.Models;
public class Bowler
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsSub { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
