using HangTab.Models;

namespace HangTab.ViewModels;

public class BusRideViewModel
{
    public BusRide BusRide { get; set; } = new();
    public BusRideWeek BusRideWeek { get; set; } = new();
}
