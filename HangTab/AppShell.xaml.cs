﻿using HangTab.Constants;
using HangTab.Views;

namespace HangTab;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(Routes.BowlerSelectSub, typeof(BowlerSelectSubPage));
        Routing.RegisterRoute(Routes.Home, typeof(CurrentWeekOverviewPage));
        Routing.RegisterRoute(Routes.PersonAdd, typeof(PersonAddEditPage));
        Routing.RegisterRoute(Routes.PersonEdit, typeof(PersonAddEditPage));
        Routing.RegisterRoute(Routes.SeasonSummary, typeof(SeasonSummaryPage));
        Routing.RegisterRoute(Routes.WeekDetails, typeof(WeekDetailsPage));
    }
}
