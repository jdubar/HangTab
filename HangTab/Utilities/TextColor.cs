namespace HangTab.Utilities;
public static class TextColor
{
    public static Color PrimaryTextColor => Application.Current?.Resources["PrimaryContrastTextColor"] as Color ?? Colors.White;
    public static Color DisabledTextColor => Application.Current?.Resources["ControlDisabledTextColor"] as Color ?? Colors.Gray;
}
