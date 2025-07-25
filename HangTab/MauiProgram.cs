﻿using CommunityToolkit.Maui;

using HangTab.Data;
using HangTab.Data.Impl;
using HangTab.Handlers.SearchBar;
using HangTab.Mappers;
using HangTab.Models;
using HangTab.Repositories;
using HangTab.Repositories.Impl;
using HangTab.Services;
using HangTab.Services.Impl;
using HangTab.ViewModels;
using HangTab.ViewModels.BottomSheets;
using HangTab.ViewModels.Items;
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
            .RegisterRepositories()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews()
            .RegisterBottomSheets()
            .RegisterMappers();

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
        builder.Services.AddSingleton<IFileSystemService>(new FileSystemService(FileSystem.Current));
        builder.Services.AddSingleton<IAudioService>(provider =>
        {
            var audioManager = AudioManager.Current;
            var fileSystemService = provider.GetRequiredService<IFileSystemService>();
            return new AudioService(audioManager, fileSystemService);
        });
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
        builder.Services.AddSingleton<CurrentWeekOverviewViewModel>();
        builder.Services.AddSingleton<PersonListOverviewViewModel>();
        builder.Services.AddSingleton<WeekListOverviewViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();

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
        builder.Services.AddSingleton<WeekListOverviewPage>();
        builder.Services.AddSingleton<SettingsPage>();

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

    private static MauiAppBuilder RegisterMappers(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IMapper<CurrentWeekListItemViewModel, Bowler>, BowlerMapper>();
        builder.Services.AddSingleton<IMapper<BowlerListItemViewModel, Person>, PersonMapper>();
        builder.Services.AddSingleton<IMapper<IEnumerable<Person>, IEnumerable<BowlerListItemViewModel>>, BowlerListItemViewModelMapper>();
        builder.Services.AddSingleton<IMapper<IEnumerable<Bowler>, IEnumerable<BowlerListItemViewModel>>, BowlerListItemViewModelMapper>();
        builder.Services.AddSingleton<IMapper<IEnumerable<Bowler>, IEnumerable<CurrentWeekListItemViewModel>>, CurrentWeekListItemViewModelMapper>();
        builder.Services.AddSingleton<IMapper<IEnumerable<Person>, IEnumerable<SubListItemViewModel>>, SubListItemViewModelMapper>();
        builder.Services.AddSingleton<IMapper<IEnumerable<Week>, IEnumerable<WeekListItemViewModel>>, WeekListItemViewModelMapper>();
        return builder;
    }
}