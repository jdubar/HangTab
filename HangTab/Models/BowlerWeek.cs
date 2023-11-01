namespace HangTab.Models;

public class BowlerWeek
{
    public Bowler Bowler { get; set; } = new();
    public Week Week { get; set; } = new();

    public BowlerWeek Clone() => MemberwiseClone() as BowlerWeek;
}
