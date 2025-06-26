using HangTab.Services.Impl;
using HangTab.Themes;

namespace HangTab.Tests.Services;

public class ThemeServiceTests
{
    [Fact]
    public void SetLightTheme_AddsLightThemeAndSetsAppThemeLight()
    {
        // Arrange
        var app = new Application();
        Application.Current = app;
        var service = new ThemeService();

        // Pre-add DarkTheme to simulate switching from dark to light
        var darkTheme = GetStaticTheme<DarkTheme>("DarkTheme");
        app.Resources.MergedDictionaries.Add(darkTheme);

        // Act
        service.SetLightTheme();

        // Assert
        Assert.Contains(app.Resources.MergedDictionaries, d => d is LightTheme);
        Assert.DoesNotContain(app.Resources.MergedDictionaries, d => d is DarkTheme);
        Assert.Equal(AppTheme.Light, app.UserAppTheme);
    }

    [Fact]
    public void SetDarkTheme_AddsDarkThemeAndSetsAppThemeDark()
    {
        // Arrange
        var app = new Application();
        Application.Current = app;
        var service = new ThemeService();

        // Pre-add LightTheme to simulate switching from light to dark
        var lightTheme = GetStaticTheme<LightTheme>("LightTheme");
        app.Resources.MergedDictionaries.Add(lightTheme);

        // Act
        service.SetDarkTheme();

        // Assert
        Assert.Contains(app.Resources.MergedDictionaries, d => d is DarkTheme);
        Assert.DoesNotContain(app.Resources.MergedDictionaries, d => d is LightTheme);
        Assert.Equal(AppTheme.Dark, app.UserAppTheme);
    }

    [Fact]
    public void SetLightTheme_ThrowsIfApplicationCurrentIsNull()
    {
        // Arrange
        Application.Current = null;
        var service = new ThemeService();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => service.SetLightTheme());
        Assert.Equal("Current application is not set.", ex.Message);
    }

    [Fact]
    public void SetDarkTheme_ThrowsIfApplicationCurrentIsNull()
    {
        // Arrange
        Application.Current = null;
        var service = new ThemeService();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => service.SetDarkTheme());
        Assert.Equal("Current application is not set.", ex.Message);
    }

    private static T GetStaticTheme<T>(string fieldName) where T : ResourceDictionary
    {
        // Use reflection to get the static LightTheme/DarkTheme instance from ThemeService
        var field = typeof(ThemeService).GetField(fieldName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        return (T)field!.GetValue(null)!;
    }
}