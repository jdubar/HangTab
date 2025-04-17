using HangTab.Enums;

using System.Globalization;

namespace HangTab.Converters;
public class BowlerStatusEnumToTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Status type)
        {
            return string.Empty;
        }

        return type switch
        {
            Status.Active => "Active bowler",
            Status.Blind => "Blind bowler",
            Status.UsingSub => "Select a sub",
            _ => string.Empty
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
