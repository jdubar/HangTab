using HangTab.DependencyInjection;

namespace HangTab;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.InitializeMauiApp();

        builder.AddCustomServices();
        builder.InitializeViews();

        builder.AddDebugLogging();

        return builder.Build();
    }
}