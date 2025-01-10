using HangTab.Models;

namespace HangTab.Services;
public interface INavigationService
{
    Task GoBack();
    Task GoToAddBowler();
    Task GoToEditBowler(Bowler bowler);
    Task GoToBowlerOverview();
    Task GoToCurrentWeekOverview();
    Task GoToSettings();
}
