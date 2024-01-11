using HangTab.Helpers;

namespace HangTab.Services;
public interface IShellService
{
    Task<string> DisplayOptionsPrompt(string title, string option1, string option2);
    Task DisplayAlert(string title, string msg, string buttonText);
    Task<bool> DisplayPrompt(string title, string msg, string accept, string cancel);
    Task DisplayToast(string text);
    Task GoToPageWithData<TTable>(ShellNavigationState state, TTable model) where TTable : class, new();
    Task ReturnToPage();
}
