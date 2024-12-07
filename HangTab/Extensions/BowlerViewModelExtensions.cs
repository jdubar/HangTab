using MvvmHelpers;

namespace HangTab.Extensions;
public static class BowlerViewModelExtensions
{
    public static void AddHanging(this BowlerViewModel vm)
    {
        vm.Bowler.HangCount++;
        vm.BowlerWeek.Hangings++;
    }

    public static void ResetForNewWeek(this ObservableRangeCollection<BowlerViewModel> bowlers, int weekNumber)
    {
        foreach (var bowler in bowlers)
        {
            bowler.BowlerWeek.Hangings = 0;
            bowler.BowlerWeek.WeekNumber = weekNumber;
            bowler.IsEnableSwitchBowler = true;
            bowler.IsEnableUndo = false;
        }
    }

    public static void SetIsLowestHangs(this ObservableRangeCollection<BowlerViewModel> bowlers)
    {
        bowlers.Where(b => !b.Bowler.IsSub).ToList().ForEach(bowler =>
            bowler.IsLowestHangs = bowler.Bowler.Hangings == bowlers.Min(y => y.Bowler.HangCount));
    }

    public static void UndoHanging(this BowlerViewModel vm)
    {
        if (vm.Bowler.HangCount == 0 || vm.BowlerWeek.Hangings == 0)
        {
            return;
        }

        vm.Bowler.HangCount--;
        vm.BowlerWeek.Hangings--;
    }
}
