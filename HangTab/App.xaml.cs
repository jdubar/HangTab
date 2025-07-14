using HangTab.Enums;
using HangTab.Services;

namespace HangTab;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the app code behind. There's no logic to test.")]
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;

        // TODO: Check how long the user has been away to save the current week stats and start a new week
        SetCurrentUserSelectedTheme();
        InitializeDatabase();
    }

    protected override Window CreateWindow(IActivationState? activationState) => new(new AppShell());

    private void InitializeDatabase()
     {
        var databaseService = _serviceProvider.GetService<IDatabaseService>();
        if (databaseService is null)
        {
            return;
        }

        databaseService.InitializeDatabase();
     }

    private void SetCurrentUserSelectedTheme()
    {
        var settingsService = _serviceProvider.GetService<ISettingsService>();
        if (settingsService is null)
        {
            return;
        }

        if (settingsService.Theme == (int)Theme.Light)
        {
            return;
        }

        var themeService = _serviceProvider.GetService<IThemeService>();
        if (themeService is null)
        {
            return;
        }

        if (settingsService.Theme == (int)Theme.Dark)
        {
            themeService.SetDarkTheme();
        }
    }
}
