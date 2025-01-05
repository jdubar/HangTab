using CommunityToolkit.Maui.Alerts;

namespace HangTab.Services.Impl;
public class DialogService : IDialogService
{
    public async Task AlertAsync(string title, string message, string buttonText) => await Shell.Current.DisplayAlert(title, message, buttonText);
    public Task<bool> Ask(string title, string message, string trueButtonText = "Yes", string falseButtonText = "No") => Shell.Current.DisplayAlert(title, message, trueButtonText, falseButtonText);

    public Task Notify(string title, string message, string buttonText = "OK") => Shell.Current.DisplayAlert(title, message, buttonText);

    public async Task ToastAsync(string text)
    {
        using var token = new CancellationTokenSource();
        var toast = Toast.Make(text);

        await toast.Show(token.Token);
    }
}
