using HangTab.Models;

namespace HangTab.ViewModels;

public class BowlerViewModel
{
    public Bowler Bowler { get; set; } = new();
    public BowlerWeek BowlerWeek { get; set; } = new();
}
