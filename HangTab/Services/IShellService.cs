﻿namespace HangTab.Services;
public interface IShellService
{
    Task DisplayAlertAsync(string title, string msg, string buttonText);
    Task<bool> DisplayPromptAsync(string title, string msg, string accept, string cancel);
    Task DisplayToastAsync(string text);
    Task GoToPageAsync(ShellNavigationState state);
    Task GoToPageWithDataAsync<TTable>(ShellNavigationState state, TTable model) where TTable : class, new();
    Task ReturnToPageAsync();
}
