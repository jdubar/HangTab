using HangTab.Enums;

using System.Globalization;

namespace HangTab.Converters;
public class BowlerTypeEnumToTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not BowlerType type)
        {
            return string.Empty;
        }

        return type switch
        {
            BowlerType.Regular => "Regular bowler",
            BowlerType.Sub => "Substitute bowler",
            _ => throw new NotImplementedException(),
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
