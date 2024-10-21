namespace HangTab.Extensions;
public static class BusRideViewModelExtensions
{
    public static void AddBusRide(this BusRideViewModel vm)
    {
        vm.BusRide.Total++;
        vm.BusRideWeek.BusRides++;
    }

    public static void UndoBusRide(this BusRideViewModel vm)
    {
        if (vm.BusRide.Total == 0 || vm.BusRideWeek.BusRides == 0)
        {
            return;
        }

        vm.BusRide.Total--;
        vm.BusRideWeek.BusRides--;
    }

    public static void ResetBusRidesForWeek(this BusRideViewModel vm, int workingWeek)
    {
        vm.BusRideWeek.BusRides = 0;
        vm.BusRideWeek.WeekNumber = workingWeek;
    }
}
