using CommunityToolkit.Maui;

using HangTab.Data;
using HangTab.Helpers;
using HangTab.ViewModels;
using HangTab.Views;

using Microsoft.Extensions.Logging;

namespace HangTab
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "GoogleFont");
                });

            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<BowlerViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<AddBowlerPage>();
            builder.Services.AddSingleton<SwitchBowlerPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}