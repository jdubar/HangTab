namespace HangTab.Models;
public class SeasonReport
{
    public IEnumerable<Bowler> AllBowlers { get; set; }
    public IEnumerable<BowlerWeek> AllBowlerWeeks { get; set; }
    public int LastSavedWeek { get; set; }
}
