using CommunityToolkit.Maui;

using HangTab.Data;
using HangTab.Data.Impl;
using HangTab.Repositories;
using HangTab.Repositories.Impl;
using HangTab.Services;
using HangTab.Services.Impl;
using HangTab.ViewModels;
using HangTab.Views;
using Microsoft.Extensions.Logging;

namespace HangTab;

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
        builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
        return builder;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
        builder.Services.AddSingleton<ISettingsService>(new SettingsService(Preferences.Default));

        builder.Services.AddTransient<IBowlerService, BowlerService>();
        builder.Services.AddTransient<IDatabaseService, DatabaseService>();
        builder.Services.AddTransient<IDialogService, DialogService>();
        builder.Services.AddTransient<INavigationService, NavigationService>();
        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<BowlerListOverviewViewModel>();
        builder.Services.AddTransient<BowlerAddEditViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        return builder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<BowlerOverviewPage>();
        builder.Services.AddTransient<BowlerAddEditPage>();
        builder.Services.AddTransient<SettingsPage>();
        return builder;
    }
}