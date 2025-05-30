using CommunityToolkit.Maui;

using HangTab.Data;
using HangTab.Data.Impl;
using HangTab.Repositories;
using HangTab.Repositories.Impl;
using HangTab.Services;
using HangTab.Services.Impl;
using HangTab.ViewModels;
using HangTab.ViewModels.BottomSheets;
using HangTab.ViewModels.Popups;
using HangTab.Views;
using HangTab.Views.Popups;

using Microsoft.Extensions.Logging;

using Plugin.Maui.Audio;

using System.Runtime.Versioning;

using The49.Maui.BottomSheet;

namespace HangTab;

public static class MauiProgram
{
    [SupportedOSPlatform("windows10.0.17763.0")]
    [SupportedOSPlatform("android21.0")]
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit(options =>
            {
                options.SetShouldEnableSnackbarOnWindows(true);
            })
            .UseBottomSheet()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIconsOutlined-Regular.otf", "MaterialIcons");
            })
            .RegisterRepositories()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews()
            .RegisterPopups();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder RegisterRepositories(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IMediaPickerRepository>(new MediaPickerRepository(MediaPicker.Default));

        builder.Services.AddTransient<IPersonRepository, PersonRepository>();
        builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
        builder.Services.AddTransient<IWeekRepository, WeekRepository>();
        builder.Services.AddTransient<IBowlerRepository, BowlerRepository>();
        return builder;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAudioService>(new AudioService(AudioManager.Current));
        builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
        builder.Services.AddSingleton<ISettingsService>(new SettingsService(Preferences.Default));

        builder.Services.AddTransient<IPersonService, PersonService>();
        builder.Services.AddTransient<IDatabaseService, DatabaseService>();
        builder.Services.AddTransient<IDialogService, DialogService>();
        builder.Services.AddTransient<IMediaPickerService, MediaPickerService>();
        builder.Services.AddTransient<INavigationService, NavigationService>();
        builder.Services.AddTransient<IThemeService, ThemeService>();
        builder.Services.AddTransient<IWeekService, WeekService>();
        builder.Services.AddTransient<IBowlerService, BowlerService>();
        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<BowlerListOverviewViewModel>();
        builder.Services.AddSingleton<CurrentWeekOverviewViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();

        builder.Services.AddTransient<AvatarSelectViewModel>();
        builder.Services.AddTransient<BowlerSelectSubViewModel>();
        builder.Services.AddTransient<PersonAddEditViewModel>();
        return builder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<BowlerOverviewPage>();
        builder.Services.AddSingleton<CurrentWeekOverviewPage>();
        builder.Services.AddSingleton<SettingsPage>();

        builder.Services.AddTransient<PersonAddEditPage>();
        builder.Services.AddTransient<BowlerSelectSubPage>();
        return builder;
    }

    private static MauiAppBuilder RegisterPopups(this MauiAppBuilder builder)
    {
        builder.Services.AddTransientPopup<BowlerTypePopup, BowlerTypePopupViewModel>();
        return builder;
    }
}