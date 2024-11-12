using CommunityToolkit.Mvvm.ComponentModel;

namespace HangTab.Models.Wrappers;
public partial class BowlerWeekWrapper : ObservableObject
{
    public BowlerWeekWrapper(BowlerWeek bowlerweek)
    {
        if (bowlerweek is null)
        {
            return;
        }

        Id = bowlerweek.Id;
        WeekNumber = bowlerweek.WeekNumber;
        BowlerId = bowlerweek.BowlerId;
        Hangings = bowlerweek.Hangings;
    }

    public int Id { get; set; }
    public int WeekNumber { get; set; }
    public int BowlerId { get; set; }

    [ObservableProperty]
    private int _hangings;
}
