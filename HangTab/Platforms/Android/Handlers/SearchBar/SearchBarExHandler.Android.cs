using Android.Graphics;
using Android.Widget;

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

using AColor = Android.Graphics.Color;

namespace HangTab.Handlers.SearchBar;
public partial class SearchBarExHandler : SearchBarHandler
{
    public void SetIconColor(AColor value)
    {
        var searchIcon = PlatformView.FindViewById(Resource.Id.search_mag_icon) as ImageView;
        searchIcon?.SetColorFilter(value, PorterDuff.Mode.SrcAtop);
    }

    public AColor GetTextColor() => VirtualView.TextColor.ToPlatform();

    public static void MapIconColor(ISearchBarHandler handler, ISearchBar searchBar)
    {
        if (handler is SearchBarExHandler customHandler)
        {
            customHandler.SetIconColor(customHandler.GetTextColor());
        }
    }
}
