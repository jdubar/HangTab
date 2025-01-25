using HangTab.Models;

namespace HangTab.Services;
public interface INavigationService
{
    Task GoBack();
    Task GoToAddBowler();
    Task GoToEditBowler(Bowler bowler);
    Task GoToSwitchBowler(WeeklyLineup weeklyLineup);
    Task GoToBowlerOverview();
    Task GoToCurrentWeekOverview();
    Task GoToSettings();
}
