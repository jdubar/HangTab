using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Enums;

namespace HangTab.ViewModels;
public partial class WeeklyLineupViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _position;

    [ObservableProperty]
    private BowlerStatus _status;

    [ObservableProperty]
    private int _hangCount;

    [ObservableProperty]
    private BowlerViewModel _bowler;

    public WeeklyLineupViewModel(
        int id,
        int position,
        BowlerStatus status,
        int hangCount,
        BowlerViewModel bowler)
    {
        Id = id;
        Position = position;
        Status = status;
        HangCount = hangCount;
        Bowler = bowler;
    }

}
