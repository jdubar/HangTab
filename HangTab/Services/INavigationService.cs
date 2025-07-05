using HangTab.Models;

namespace HangTab.Services;
public interface INavigationService
{
    Task GoBack();
    Task GoToAddBowler();
    Task GoToEditBowler(Person person);
    Task GoToSeasonSummary();
    Task GoToSelectSub(Bowler bowler);
    Task GoToWeekDetails(int weekId);
}
