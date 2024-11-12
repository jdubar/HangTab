using HangTab.Models.Wrappers;
using SQLite;

namespace HangTab.Models;

public class Bowler
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; private set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsHidden { get; set; }
    public bool IsSub { get; set; }
    public int TotalHangings { get; set; }

    public static Bowler GenerateNewFromWrapper(BowlerWrapper wrapper)
    {
        return new Bowler
        {
            Id = wrapper.Id,
            ImageUrl = wrapper.ImageUrl,
            FirstName = wrapper.FirstName,
            LastName = wrapper.LastName,
            IsHidden = wrapper.IsHidden,
            IsSub = wrapper.IsSub,
            TotalHangings = wrapper.TotalHangings,
        };
    }
}
