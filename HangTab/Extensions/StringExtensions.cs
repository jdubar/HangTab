using System.Text;

namespace HangTab.Extensions;
internal static class StringExtensions
{
    internal static string GetInitials(this string name)
    {
        var nameSplit = name.Split([",", " "], StringSplitOptions.RemoveEmptyEntries);
        var stringBuilder = new StringBuilder();
        foreach (var item in nameSplit)
        {
            stringBuilder.Append(item[..1].ToUpper(System.Globalization.CultureInfo.CurrentCulture));
        }

        return stringBuilder.ToString();           
    }
}
