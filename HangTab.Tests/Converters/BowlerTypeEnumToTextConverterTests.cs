using HangTab.Converters;
using HangTab.Enums;

using System.Globalization;

namespace HangTab.Tests.Converters;

public class BowlerTypeEnumToTextConverterTests
{
    [Theory]
    [InlineData(BowlerType.Regular, "Regular bowler")]
    [InlineData(BowlerType.Sub, "Substitute bowler")]
    public void Convert_ValidBowlerType_ReturnsExpectedText(BowlerType type, string expected)
    {
        // Act
        var converter = new BowlerTypeEnumToTextConverter();
        var result = converter.Convert(type, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_InvalidType_ReturnsEmptyString()
    {
        // Act
        var converter = new BowlerTypeEnumToTextConverter();
        var result = converter.Convert(123, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Convert_NullValue_ReturnsEmptyString()
    {
        // Act
        var converter = new BowlerTypeEnumToTextConverter();
        var result = converter.Convert(null, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Convert_UnknownEnum_ThrowsNotImplementedException()
    {
        // Arrange
        var unknown = (BowlerType)999;
        var converter = new BowlerTypeEnumToTextConverter();

        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            converter.Convert(unknown, typeof(string), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ConvertBack_Always_ThrowsNotImplementedException()
    {
        // Arrange
        var converter = new BowlerTypeEnumToTextConverter();

        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            converter.ConvertBack("Regular bowler", typeof(BowlerType), null, CultureInfo.InvariantCulture));
    }
}