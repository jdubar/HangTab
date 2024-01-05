using HangTab.Models;

namespace HangTab.ViewModels;
public class WeekViewModel
{
    public int WeekNumber { get; set; }
    public int TotalHangings { get; set; }
    public int TotalBusRides { get; set; }
    public IEnumerable<Bowler> Bowlers { get; set; }
}
