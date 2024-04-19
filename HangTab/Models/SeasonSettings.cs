using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace HangTab.Models;

[Table("seasonsettings")]
public class SeasonSettings : ObservableObject
{
    [PrimaryKey]
    public int Id { get; set; } = 1;

    public int TotalSeasonWeeks { get; set; } = 34;
}
