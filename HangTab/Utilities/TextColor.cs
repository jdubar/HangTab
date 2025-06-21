namespace HangTab.Utilities;
public static class TextColor
{
    public static Color PrimaryContrastTextColor => Application.Current?.Resources["PrimaryContrastTextColor"] as Color ?? Colors.White;
    public static Color ControlDisabledTextColor => Application.Current?.Resources["ControlDisabledTextColor"] as Color ?? Colors.Gray;
}
