using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Bowlers))]
    private List<BowlerListItemViewModel> _allBowlers = [];

    public ObservableCollection<BowlerListItemViewModel> Bowlers => new(AllBowlers);

    public WeekListItemViewModel(
        int id,
        int number,
        int busRides,
        int hangings,
        List<BowlerListItemViewModel> allBowlers)
    {
        Id = id;
        Number = number;
        BusRides = busRides;
        Hangings = hangings;
        AllBowlers = allBowlers;
    }
}
