using HangTab.Enums;
using HangTab.Resources.Styles;
using HangTab.Themes;

namespace HangTab.Services.Impl;

public class ThemeService : IThemeService
{
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
            mergedDictionaries.Clear();

            if (theme == Theme.Dark)
            {
                mergedDictionaries.Add(new DarkTheme());
            }
            else
            {
                mergedDictionaries.Add(new LightTheme());
            }
            mergedDictionaries.Add(new Icons());
            mergedDictionaries.Add(new CustomStyles());
        }
    }
}
