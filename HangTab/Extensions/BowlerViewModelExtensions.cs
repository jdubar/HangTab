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
        vm.Bowler.TotalHangings--;
        vm.BowlerWeek.Hangings--;
    }
}
