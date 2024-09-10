using HangTab.Views;
using HangTab.Views.ViewModels;

namespace HangTab.DependencyInjection;
public static class UserInterface
{
    public static void InitializeViews(this MauiAppBuilder builder)
    {
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
    }
}
