﻿namespace HangTab.Services.Impl;
public class ShellService : IShellService
{
    public async Task DisplayAlert(string title, string msg, string buttonText) =>
        await Shell.Current.DisplayAlert(title, msg, buttonText);

    public async Task<bool> DisplayPrompt(string title, string msg, string accept, string cancel) =>
        await Shell.Current.DisplayAlert(title, msg, accept, cancel);

    public async Task ReturnToPage() =>
        await Shell.Current.GoToAsync("..", true);

    public async Task GoToPage(ShellNavigationState state, ShellNavigationQueryParameters pairs = null)
    {
        if (pairs == null)
        {
            await Shell.Current.GoToAsync(state, true);
        }
        else
        {
            await Shell.Current.GoToAsync(state, true, pairs);
        }
    }
}