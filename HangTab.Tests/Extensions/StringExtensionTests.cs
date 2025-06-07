using HangTab.Extensions;

namespace HangTab.Tests.ExtensionTests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("John Doe", "JD")]
    [InlineData("Jane", "J")]
    [InlineData("John A. Doe", "JAD")]
    [InlineData("john doe", "JD")]
    [InlineData("john   doe", "JD")]
    [InlineData("John,Doe", "JD")]
    [InlineData("John, Doe", "JD")]
    [InlineData(" John  Doe ", "JD")]
    [InlineData("A B C", "ABC")]
    [InlineData("A", "A")]
    [InlineData("A,B,C", "ABC")]
    [InlineData("A, B, C", "ABC")]
    public void GetInitials_ShouldReturnExpected(string input, string expected)
    {
        var actual = input.GetInitials();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void GetInitials_ShouldReturnEmpty(string input)
    {
        var expected = string.Empty;
        var actual = input.GetInitials();
        Assert.Equal(expected, actual);
    }
}
