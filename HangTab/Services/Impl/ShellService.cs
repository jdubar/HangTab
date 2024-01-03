using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

using HangTab.Models;

namespace HangTab.Services.Impl;
public class ShellService : IShellService
{
    public async Task DisplayAlert(string title, string msg, string buttonText) =>
        await Shell.Current.DisplayAlert(title, msg, buttonText);

    public async Task<bool> DisplayPrompt(string title, string msg, string accept, string cancel) =>
        await Shell.Current.DisplayAlert(title, msg, accept, cancel);

    public async Task DisplayToast(string text)
    {
        using var token = new CancellationTokenSource();
        var duration = ToastDuration.Short;
        var fontSize = 14;

        var toast = Toast.Make(text, duration, fontSize);

        await toast.Show(token.Token);
    }

    public async Task GoToPageWithData(ShellNavigationState state, Bowler bowler)
    {
        bowler ??= new Bowler();
        var navParam = new ShellNavigationQueryParameters
        {
            { "Bowler", bowler }
        };
        await Shell.Current.GoToAsync(state, true, navParam);
    }

    public async Task ReturnToPage() =>
        await Shell.Current.GoToAsync("..", true);
}
