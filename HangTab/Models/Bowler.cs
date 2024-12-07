using HangTab.Models.Wrappers;

using SQLite;

using SQLiteNetExtensions.Attributes;

namespace HangTab.Models;

public class Bowler
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; private set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsSub { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    [ForeignKey(typeof(Lineup), Name = "Id")]
    public int LineupId { get; set; }

    public static Bowler GenerateNewFromWrapper(LineupWrapper wrapper)
    {
        return new Bowler
        {
            Id = wrapper.Id,
            ImageUrl = wrapper.Bowler.ImageUrl,
            FirstName = wrapper.Bowler.FirstName,
            LastName = wrapper.Bowler.LastName,
            LineupId = wrapper.BowlerByWeekId,
        };
    }
}