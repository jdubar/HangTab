using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace HangTab.Services.Impl;
public class ShellService : IShellService
{
    public async Task<string> DisplayOptionsPromptAsync(string title, string option1, string option2) =>
        await Shell.Current.DisplayActionSheet(title, "Cancel", null, option1, option2);

    public async Task DisplayAlertAsync(string title, string msg, string buttonText) =>
        await Shell.Current.DisplayAlert(title, msg, buttonText);

    public async Task<bool> DisplayPromptAsync(string title, string msg, string accept, string cancel) =>
        await Shell.Current.DisplayAlert(title, msg, accept, cancel);

    public async Task DisplayToastAsync(string text)
    {
        using var token = new CancellationTokenSource();
        var duration = ToastDuration.Short;
        var fontSize = 14;

        var toast = Toast.Make(text, duration, fontSize);

        await toast.Show(token.Token);
    }

    public async Task GoToPageAsync(ShellNavigationState state) =>
        await Shell.Current.GoToAsync(state, true);

    public async Task GoToPageWithDataAsync<TTable>(ShellNavigationState state, TTable model) where TTable : class, new()
    {
        model ??= new TTable();
        var navParam = new ShellNavigationQueryParameters
        {
            { typeof(TTable).Name, model }
        };
        await Shell.Current.GoToAsync(state, true, navParam);
    }

    public async Task ReturnToPageAsync() =>
        await Shell.Current.GoToAsync("..", true);
}
