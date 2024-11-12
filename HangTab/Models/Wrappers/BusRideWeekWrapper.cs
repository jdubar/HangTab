using CommunityToolkit.Mvvm.ComponentModel;

namespace HangTab.Models.Wrappers;
public partial class BusRideWeekWrapper : ObservableObject
{
    public BusRideWeekWrapper(BusRideWeek busRideWeek)
    {
        if (busRideWeek is null)
        {
            return;
        }

        Id = busRideWeek.Id;
        WeekNumber = busRideWeek.WeekNumber;
        BusRides = busRideWeek.BusRides;
    }

    public int Id { get; set; }
    public int WeekNumber { get; set; }
    [ObservableProperty]
    private int _busRides;
}
