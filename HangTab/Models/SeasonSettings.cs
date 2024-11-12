using SQLite;

namespace HangTab.Models;

public class SeasonSettings
{
    [PrimaryKey]
    public int Id { get; private init; } = 1;

    public decimal CostPerHang { get; set; } = 0.25m;
    public int CurrentSeasonWeek { get; set; } = 1;
    public int TotalSeasonWeeks { get; set; } = 34;
}
