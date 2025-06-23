using CommunityToolkit.Mvvm.ComponentModel;

namespace HangTab.ViewModels.Items;
public partial class WeekListItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _number;

    [ObservableProperty]
    private int _busRides;

    [ObservableProperty]
    private int _hangCount;

    public WeekListItemViewModel(
        int id,
        int number,
        int busRides,
        int hangCount)
    {
        Id = id;
        Number = number;
        BusRides = busRides;
        HangCount = hangCount;
    }
}
