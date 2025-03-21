using HangTab.Enums;

namespace HangTab.Services.Impl;

public class ThemeService : IThemeService
{
    public void SetLightTheme() => SetTheme(Theme.Light);
    public void SetDarkTheme() => SetTheme(Theme.Dark);

    private static void SetTheme(Theme theme)
    {
        if (Application.Current is null)
        {
            throw new InvalidOperationException("Current application is not set.");
        }

        Application.Current.UserAppTheme = theme == Theme.Dark
            ? AppTheme.Dark
            : AppTheme.Light;
    }
}
