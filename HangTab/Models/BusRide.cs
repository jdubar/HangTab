using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace HangTab.Models;

[Table("busride")]
public class BusRide : ObservableObject
{
    private int _total;

    [PrimaryKey]
    public int Id { get; set; } = 1;
    public int Total
    {
        get => _total;
        set => SetProperty(ref _total, value);
    }
}
