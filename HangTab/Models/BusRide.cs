using SQLite;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangTab.Models;

[Table("busride")]
public class BusRide : INotifyPropertyChanged
{
    private int _totalBusRides;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int TotalBusRides
    {
        get => _totalBusRides;
        set
        {
            _totalBusRides = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
