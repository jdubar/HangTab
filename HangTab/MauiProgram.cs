using CommunityToolkit.Maui;

using HangTab.Data;
using HangTab.Services;
using HangTab.Services.Impl;
using HangTab.Views;
using HangTab.Views.ViewModels;

using Microsoft.Extensions.Logging;

using Plugin.Maui.Audio;

using SkiaSharp.Views.Maui.Controls.Hosting;

namespace HangTab;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
               .UseMauiCommunityToolkit()
               .UseSkiaSharp()
               .ConfigureFonts(fonts =>
               {
                   fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                   fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                   fonts.AddFont("MaterialIconsOutlined-Regular.ttf", "GoogleFont");
               });

        builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
        builder.Services.AddSingleton(AudioManager.Current);

        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
        builder.Services.AddSingleton<IMediaService, MediaService>();
        builder.Services.AddSingleton<IShellService, ShellService>();

        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<HomeViewModel>();

        builder.Services.AddSingleton<ManageBowlersPage>();
        builder.Services.AddSingleton<ManageBowlerViewModel>();

        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<SettingsViewModel>();

        builder.Services.AddSingleton<AddBowlerPage>();
        builder.Services.AddSingleton<AddBowlerViewModel>();

        builder.Services.AddSingleton<SwitchBowlerPage>();
        builder.Services.AddSingleton<SwitchBowlerViewModel>();

        builder.Services.AddSingleton<SeasonPage>();
        builder.Services.AddSingleton<SeasonViewModel>();

        builder.Services.AddSingleton<WeekDetailsPage>();
        builder.Services.AddSingleton<WeekDetailsViewModel>();

        builder.Services.AddSingleton<SeasonSummaryPage>();
        builder.Services.AddSingleton<SeasonSummaryViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}