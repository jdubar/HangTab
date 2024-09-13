#if DEBUG
using Microsoft.Extensions.Logging;
#endif

namespace HangTab.DependencyInjection;
public static class ClientLogging
{
    public static void AddLogging(this MauiAppBuilder builder)
    {
#if DEBUG
        builder.Logging.AddDebug();
#endif
    }
}
