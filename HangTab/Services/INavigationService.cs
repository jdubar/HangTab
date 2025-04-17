using HangTab.Models;

namespace HangTab.Services;
public interface INavigationService
{
    Task GoBack();
    Task GoToAddBowler();
    Task GoToEditBowler(Person person);
    Task GoToSwitchBowler(Bowler bowler);
    Task GoToBowlerOverview();
    Task GoToCurrentWeekOverview();
    Task GoToSettings();
}
