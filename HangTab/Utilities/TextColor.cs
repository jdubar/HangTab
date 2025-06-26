namespace HangTab.Utilities;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test text colors. There's no logic to test.")]
public static class TextColor
{
    public static Color PrimaryContrastTextColor => Application.Current?.Resources["PrimaryContrastTextColor"] as Color ?? Colors.White;
    public static Color ControlDisabledTextColor => Application.Current?.Resources["ControlDisabledTextColor"] as Color ?? Colors.Gray;
}
