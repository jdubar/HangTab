#if DEBUG
using Microsoft.Extensions.Logging;
#endif

namespace HangTab.DependencyInjection;
public static class ClientLogging
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is only used during debugging. Coverage not necessary.")]
    public static void AddDebugLogging(this MauiAppBuilder builder)
    {
#if DEBUG
        builder.Logging.AddDebug();
#endif
    }
}
