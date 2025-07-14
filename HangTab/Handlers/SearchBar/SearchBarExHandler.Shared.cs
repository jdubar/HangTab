using Microsoft.Maui.Handlers;

namespace HangTab.Handlers.SearchBar;
public partial class SearchBarExHandler : SearchBarHandler
{
#if ANDROID || WINDOWS
    public static readonly IPropertyMapper<ISearchBar, SearchBarHandler> CustomMapper =
        new PropertyMapper<ISearchBar, SearchBarHandler>(Mapper)
        {
            ["IconColor"] = MapIconColor,
        };

    public SearchBarExHandler() : base(CustomMapper, CommandMapper)
    {
    }
#endif
}
