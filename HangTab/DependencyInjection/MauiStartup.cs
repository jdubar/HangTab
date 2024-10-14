using Plugin.Maui.Audio;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace HangTab.DependencyInjection;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public static class MauiStartup
{
    public static void InitializeMauiApp(this MauiAppBuilder builder)
    {
        builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .AddAudio()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
    }
}
