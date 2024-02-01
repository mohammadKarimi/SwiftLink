using FluentAssertions;
using SwiftLink.Application.Common;
using Xunit;

namespace SwiftLink.Application.UnitTests.Common;

public class HashBasedShortCodeGeneratorTests
{
    private readonly HashBasedShortCodeGenerator _generator = new();

    [Theory]
    [InlineData("https://www.example.com")]
    [InlineData("https://www.example.com/test")]
    [InlineData("https://anotherexample.com")]
    public void Generate_ShouldReturnValidShortCode(string originalUrl)
    {
        var generator = new HashBasedShortCodeGenerator();
        var shortCode = generator.Generate(originalUrl);
        shortCode.Should().NotBeNull();
        shortCode.Length.Should().Be(16);
    }

    [Fact]
    public void Generate_ShouldReturnDifferentShortCodesForDifferentInputUrls()
    {
        var shortCode1 = _generator.Generate("https://www.example.com");
        var shortCode2 = _generator.Generate("https://anotherexample.com");
        shortCode1.Should().NotBe(shortCode2);
    }

    //[Fact]
    //public void GetRandomString_ShouldReturnStringWithCorrectLength()
    //{
    //    var length = 10;
    //    var randomString = _generator.GetRandomString(length);
    //   randomString.Length.Should().Be(length);
    //}

    //[Fact]
    //public void GetHash_ShouldReturnValidHash()
    //{
    //    var input = "testinput";
    //    var hash = _generator.GetHash(input);
    //    var expectedHash = BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(input)))
    //                                  .Replace("-", string.Empty)[..HashBasedShortCodeGenerator.COUNT_OF_HASH_SPLITTER];
    //    hash.Should().Be(expectedHash);
    //}
}
