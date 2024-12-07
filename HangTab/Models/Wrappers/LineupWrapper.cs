using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Enums;

namespace HangTab.Models.Wrappers;
public partial class LineupWrapper : ObservableObject
{
    public LineupWrapper(Lineup lineup, Bowler bowler)
    {
        if (bowler is null)
        {
            return;
        }

        Id = lineup.Id;
        Bowler = bowler;
        Status = lineup.Status;
        HangCount = lineup.HangCount;
        WeekId = lineup.WeekId;
    }

    public int Id { get; set; }

    [ObservableProperty]
    private Bowler _bowler;

    [ObservableProperty]
    private BowlerStatus _status;

    [ObservableProperty]
    private int _hangCount;

    [ObservableProperty]
    private int _weekId;
}
