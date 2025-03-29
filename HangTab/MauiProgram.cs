using CommunityToolkit.Maui;

using HangTab.Data;
using HangTab.Data.Impl;
using HangTab.Repositories;
using HangTab.Repositories.Impl;
using HangTab.Services;
using HangTab.Services.Impl;
using HangTab.ViewModels;
using HangTab.ViewModels.Popups;
using HangTab.Views;
using HangTab.Views.Popups;

using Microsoft.Extensions.Logging;

using Plugin.Maui.Audio;

using The49.Maui.BottomSheet;

namespace HangTab;

public static class MauiProgram
{
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

        builder.Services.AddTransient<IBowlerRepository, BowlerRepository>();
        builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
        builder.Services.AddTransient<IWeekRepository, WeekRepository>();
        builder.Services.AddTransient<IWeeklyLineupRepository, WeeklyLineupRepository>();
        return builder;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAudioService>(new AudioService(AudioManager.Current));
        builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
        builder.Services.AddSingleton<ISettingsService>(new SettingsService(Preferences.Default));

        builder.Services.AddTransient<IBowlerService, BowlerService>();
        builder.Services.AddTransient<IDatabaseService, DatabaseService>();
        builder.Services.AddTransient<IDialogService, DialogService>();
        builder.Services.AddTransient<IMediaPickerService, MediaPickerService>();
        builder.Services.AddTransient<INavigationService, NavigationService>();
        builder.Services.AddTransient<IThemeService, ThemeService>();
        builder.Services.AddTransient<IWeekService, WeekService>();
        builder.Services.AddTransient<IWeeklyLineupService, WeeklyLineupService>();
        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<BowlerListOverviewViewModel>();
        builder.Services.AddSingleton<CurrentWeekOverviewViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();

        builder.Services.AddTransient<AvatarSelectViewModel>();
        builder.Services.AddTransient<BowlerAddEditViewModel>();
        builder.Services.AddTransient<BowlerSwitchViewModel>();
        return builder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<BowlerOverviewPage>();
        builder.Services.AddSingleton<CurrentWeekOverviewPage>();
        builder.Services.AddSingleton<SettingsPage>();

        builder.Services.AddTransient<BowlerAddEditPage>();
        builder.Services.AddTransient<BowlerSwitchPage>();
        return builder;
    }

    private static MauiAppBuilder RegisterPopups(this MauiAppBuilder builder)
    {
        builder.Services.AddTransientPopup<BowlerTypePopup, BowlerTypePopupViewModel>();
        builder.Services.AddTransientPopup<DataResetPopUp, DataResetPopUpViewModel>();
        builder.Services.AddTransientPopup<DeleteBowlerPopup, DeleteBowlerPopupViewModel>();
        builder.Services.AddTransientPopup<StartNewSeasonPopup, StartNewSeasonPopupViewModel>();
        return builder;
    }
}