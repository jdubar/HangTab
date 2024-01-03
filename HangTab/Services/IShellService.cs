using HangTab.Models;

namespace HangTab.Services;
public interface IShellService
{
    Task DisplayAlert(string title, string msg, string buttonText);
    Task<bool> DisplayPrompt(string title, string msg, string accept, string cancel);
    Task DisplayToast(string text);
    Task GoToPageWithData(ShellNavigationState state, Bowler bowler);
    Task ReturnToPage();
}
