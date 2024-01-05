namespace HangTab.Services;
public interface IShellService
{
    Task DisplayAlert(string title, string msg, string buttonText);
    Task<bool> DisplayPrompt(string title, string msg, string accept, string cancel);
    Task DisplayToast(string text);
    Task GoToPageWithData<TTable>(ShellNavigationState state, TTable model) where TTable : class, new();
    Task ReturnToPage();
}
