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
    private ObservableCollection<BowlerListItemViewModel> _bowlers = [];
}
