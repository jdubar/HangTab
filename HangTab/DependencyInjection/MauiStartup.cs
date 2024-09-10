using SkiaSharp.Views.Maui.Controls.Hosting;

namespace HangTab.DependencyInjection;
public static class MauiStartup
{
    public static void InitializeMauiApp(this MauiAppBuilder builder)
    {
        builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
    }
}
