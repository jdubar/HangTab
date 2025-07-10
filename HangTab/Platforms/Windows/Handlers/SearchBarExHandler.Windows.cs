using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

using WColor = Windows.UI.Color;

namespace HangTab.Handlers.SearchBar;
public partial class SearchBarExHandler : SearchBarHandler
{
    public void SetIconColor(WColor value)
    {
        PlatformView.QueryIcon.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(value);
    }

    private WColor GetTextColor() => VirtualView.TextColor.ToWindowsColor();

    public static void MapIconColor(ISearchBarHandler handler, ISearchBar searchBar)
    {
        if (handler is SearchBarExHandler customHandler)
        {
            customHandler.SetIconColor(customHandler.GetTextColor());
        }
    }
}
