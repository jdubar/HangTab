using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace HangTab.Models;

[Table("busrideweek")]
public class BusRideWeek : ObservableObject
{
    private int _busRides;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int WeekNumber { get; set; }
    public int BusRideId { get; set; }
    public int BusRides
    {
        get => _busRides;
        set => SetProperty(ref _busRides, value);
    }
}
