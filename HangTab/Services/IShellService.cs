namespace HangTab.Services;
public interface IShellService
{
    Task DisplayAlert(string title, string msg, string buttonText);
    Task<bool> DisplayPrompt(string title, string msg, string accept, string cancel);
    Task GoToPage(ShellNavigationState state, bool animate);
    Task ReturnToPage();
}
