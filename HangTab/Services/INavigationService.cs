using HangTab.Models;

namespace HangTab.Services;
public interface INavigationService
{
    Task GoBack();
    Task GoToAddBowler();
    Task GoToEditBowler(Person person);
    Task GoToSelectSub(Bowler bowler);
}
