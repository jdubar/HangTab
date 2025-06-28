using System.Text;

namespace HangTab.Extensions;
public static class StringExtensions
{
    public static string GetInitials(this string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }

        var nameSplit = name.Split([",", " "], StringSplitOptions.RemoveEmptyEntries);
        var stringBuilder = new StringBuilder();
        foreach (var item in nameSplit)
        {
            stringBuilder.Append(item[..1].ToUpper(System.Globalization.CultureInfo.CurrentCulture));
        }

        return stringBuilder.ToString();           
    }
}
