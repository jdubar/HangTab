using CommunityToolkit.Mvvm.ComponentModel;

namespace HangTab.Models.Wrappers;
public partial class BusRideWrapper : ObservableObject
{
    public BusRideWrapper(BusRide busRide)
    {
        if (busRide is null)
        {
            return;
        }

        Id = busRide.Id;
        Total = busRide.Total;
    }

    public int Id { get; set; }

    [ObservableProperty]
    private decimal _total;
}
