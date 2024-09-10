using Microsoft.Extensions.Logging;

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
