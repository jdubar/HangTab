namespace HangTab.Extensions;
public static class BowlerViewModelExtensions
{
    public static void AddHanging(this BowlerViewModel vm)
    {
        vm.Bowler.TotalHangings++;
        vm.BowlerWeek.Hangings++;
    }

    public static void UndoHanging(this BowlerViewModel vm)
    {
        if (vm.Bowler.TotalHangings == 0 || vm.BowlerWeek.Hangings == 0)
        {
            return;
        }

        vm.Bowler.TotalHangings--;
        vm.BowlerWeek.Hangings--;
    }
}
