namespace HangTab.ViewModels.Items.Interfaces;
public interface ILowestHangCountBowler
{
    bool IsSub { get; }
    int HangCount { get; }
    bool HasLowestHangCount { get; set; }
}
