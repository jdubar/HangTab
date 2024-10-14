using HangTab.DependencyInjection;

namespace HangTab;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
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