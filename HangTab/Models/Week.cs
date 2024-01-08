using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

using SQLiteNetExtensions.Attributes;

namespace HangTab.Models;

[Table("week")]
public class Week : ObservableObject
{
    private int _busRides;

    [PrimaryKey, AutoIncrement]
    public int WeekNumber { get; set; } = 1;

    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public IEnumerable<Bowler> Bowlers { get; set; } = [];
    public int BusRides
    {
        get => _busRides;
        set => SetProperty(ref _busRides, value);
    }
}
