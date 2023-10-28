using SQLite;

namespace HangTab.Models;

[Table("bowler")]
public class Bowler
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ImageUrl { get; set; } = "account_circle.png";
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsSub { get; set; }
    public int TotalHangings { get; set; }
    public string FullName => $"{FirstName} {LastName}";

    public Bowler Clone => MemberwiseClone() as Bowler;

    public (bool IsValid, string ErrorMessage) Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            return (false, $"{nameof(FirstName)} is required.");
        }
        else if (string.IsNullOrWhiteSpace(LastName))
        {
            return (false, $"{nameof(LastName)} is required.");
        }
        return (true, string.Empty);
    }
}
