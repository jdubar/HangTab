namespace HangTab.Services;
public interface IDialogService
{
    Task AlertAsync(string title, string message, string buttonText);
    Task<bool> Ask(string title, string message, string trueButtonText = "Yes", string falseButtonText = "No");
    Task Notify(string title, string message, string buttonText = "OK");
    Task ToastAsync(string text);
}
