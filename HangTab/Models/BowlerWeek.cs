using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace HangTab.Models;

[Table("bowlerweek")]
public class BowlerWeek : ObservableObject
{
    private int _hangings;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int BowlerId { get; set; }
    public int Hangings
    {
        get => _hangings;
        set => SetProperty(ref _hangings, value);
    }
}
