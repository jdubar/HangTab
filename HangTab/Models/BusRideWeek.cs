using SQLite;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangTab.Models;

[Table("busrideweek")]
public class BusRideWeek : INotifyPropertyChanged
{
    private int _busRides;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int WeekNumber { get; set; }
    public int BusRideId { get; set; }
    public int BusRides
    {
        get => _busRides;
        set
        {
            _busRides = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
