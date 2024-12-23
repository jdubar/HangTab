using HangTab.Repositories;
using HangTab.Repositories.Impl;
using HangTab.Services.Impl;
using HangTab.ViewModels;

using Microsoft.Extensions.Logging;

namespace HangTab;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
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
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsRegular");
            })
            .RegisterRepositories()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder RegisterRepositories(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IBowlerRepository, BowlerRepository>();
        builder.Services.AddTransient<IWeekRepository, WeekRepository>();
        return builder;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IBowlerService, BowlerService>();
        builder.Services.AddTransient<INavigationService, NavigationService>();
        builder.Services.AddTransient<IWeekService, WeekService>();
        builder.Services.AddTransient<IDialogService, DialogService>();

        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<WeekOverviewViewModel>();
        //builder.Services.AddTransient<EventDetailViewModel>();
        //builder.Services.AddTransient<EventAddEditViewModel>();

        return builder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        //builder.Services.AddSingleton<EventOverviewPage>();
        //builder.Services.AddTransient<EventDetailPage>();
        //builder.Services.AddTransient<EventAddEditPage>();

        return builder;
    }
}