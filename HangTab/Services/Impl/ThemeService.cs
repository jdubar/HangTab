using HangTab.Enums;
using HangTab.Themes;

namespace HangTab.Services.Impl;

public class ThemeService : IThemeService
{
    private static readonly LightTheme LightTheme = [];
    private static readonly DarkTheme DarkTheme = [];

    public void SetLightTheme() => ResetTheme(Theme.Light);
    public void SetDarkTheme() => ResetTheme(Theme.Dark);
    
    private static void ResetTheme(Theme theme)
    {
        if (Application.Current is null)
        {
            throw new InvalidOperationException("Current application is not set.");
        }

        var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries is not null)
        {
            if (theme == Theme.Dark)
            {
                mergedDictionaries.Remove(LightTheme);
                mergedDictionaries.Add(DarkTheme);
            }
            else
            {
                mergedDictionaries.Remove(DarkTheme);
                mergedDictionaries.Add(LightTheme);
            }
        }
    }
}
