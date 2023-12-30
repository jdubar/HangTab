using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace HangTab.Models;

[Table("busride")]
public class BusRide : ObservableObject
{
    private int _totalBusRides;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int TotalBusRides
    {
        get => _totalBusRides;
        set => SetProperty(ref _totalBusRides, value);
    }
}
