using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Data;
using HangTab.Data.Impl;
using HangTab.Handlers.SearchBar;
using HangTab.Repositories;
using HangTab.Repositories.Impl;
using HangTab.Services;
using HangTab.Services.Impl;
using HangTab.ViewModels;
using HangTab.ViewModels.BottomSheets;
using HangTab.Views;
using HangTab.Views.BottomSheets;

using Microsoft.Extensions.Logging;

using Plugin.Maui.Audio;
using Plugin.Maui.BottomSheet.Hosting;

using System.Runtime.Versioning;

namespace HangTab;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the app code behind. There's no logic to test.")]
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
#if ANDROID
            .UseBottomSheet(config => config.CopyPagePropertiesToBottomSheet = true)
#endif
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIconsOutlined-Regular.otf", "MaterialIcons");
            })
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<SearchBar, SearchBarExHandler>();
            })
            .RegisterDatabaseContext()
            .RegisterRepositories()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews()
            .RegisterBottomSheets();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder RegisterDatabaseContext(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
        return builder;
    }

    private static MauiAppBuilder RegisterRepositories(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAudioRepository>(new AudioRepository(AudioManager.Current));
        builder.Services.AddSingleton<IMediaPickerRepository>(new MediaPickerRepository(MediaPicker.Default));
        builder.Services.AddSingleton<IScreenshotRepository>(new ScreenshotRepository(Screenshot.Default));
        builder.Services.AddSingleton<IShareRepository>(new ShareRepository(Share.Default));
        builder.Services.AddSingleton<IStorageRepository>(new StorageRepository(FileSystem.Current));

        builder.Services.AddTransient<IBowlerRepository, BowlerRepository>();
        builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
        builder.Services.AddTransient<IPersonRepository, PersonRepository>();
        builder.Services.AddTransient<IWeekRepository, WeekRepository>();
        return builder;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ISettingsService>(new SettingsService(Preferences.Default));
        builder.Services.AddSingleton<IMessenger, WeakReferenceMessenger>();

        builder.Services.AddTransient<IAudioService, AudioService>();
        builder.Services.AddTransient<IBowlerService, BowlerService>();
        builder.Services.AddTransient<IDatabaseService, DatabaseService>();
        builder.Services.AddTransient<IMediaPickerService, MediaPickerService>();
        builder.Services.AddTransient<IPersonService, PersonService>();
        builder.Services.AddTransient<IScreenshotService, ScreenshotService>();
        builder.Services.AddTransient<IShareService, ShareService>();
        builder.Services.AddTransient<IWeekService, WeekService>();

        builder.Services.AddTransient<IDialogService, DialogService>();
        builder.Services.AddTransient<INavigationService, NavigationService>();
        builder.Services.AddTransient<IThemeService, ThemeService>();
        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<CurrentWeekOverviewViewModel>();
        builder.Services.AddSingleton<PersonListOverviewViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<WeekListOverviewViewModel>();

        builder.Services.AddTransient<AvatarSelectViewModel>();
        builder.Services.AddTransient<BowlerSelectSubViewModel>();
        builder.Services.AddTransient<DataManagerViewModel>();
        builder.Services.AddTransient<PersonAddEditViewModel>();
        builder.Services.AddTransient<SeasonSummaryViewModel>();
        builder.Services.AddTransient<WeekDetailsViewModel>();
        return builder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<CurrentWeekOverviewPage>();
        builder.Services.AddSingleton<PersonListOverviewPage>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<WeekListOverviewPage>();

        builder.Services.AddTransient<BowlerSelectSubPage>();
        builder.Services.AddTransient<PersonAddEditPage>();
        builder.Services.AddTransient<SeasonSummaryPage>();
        builder.Services.AddTransient<WeekDetailsPage>();
        return builder;
    }

    private static MauiAppBuilder RegisterBottomSheets(this MauiAppBuilder builder)
    {
        builder.Services.AddBottomSheet<AvatarSelectBottomSheet, AvatarSelectViewModel>(nameof(AvatarSelectBottomSheet));
        return builder;
    }
}