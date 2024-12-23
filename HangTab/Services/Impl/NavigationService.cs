namespace HangTab.Services.Impl;
public class NavigationService : INavigationService
{
    public Task GoBack() => Shell.Current.GoToAsync("..");
}
