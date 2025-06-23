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
    private int _hangings;

    public WeekListItemViewModel(
        int id,
        int number,
        int busRides,
        int hangings)
    {
        Id = id;
        Number = number;
        BusRides = busRides;
        Hangings = hangings;
    }
}
