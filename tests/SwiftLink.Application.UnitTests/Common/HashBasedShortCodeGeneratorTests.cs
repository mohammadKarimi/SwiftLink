using FluentAssertions;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SwiftLink.Application.UnitTests.Common;

public class HashBasedShortCodeGeneratorTests(ITestOutputHelper testOutput)
{
    private readonly HashBasedShortCodeGenerator _generator = new();
    private readonly ITestOutputHelper _output = testOutput;

    [Theory]
    [InlineData("https://www.example.com")]
    [InlineData("https://www.example.com/test")]
    [InlineData("https://anotherexample.com")]
    public void Generate_ShouldReturnValidShortCode(string originalUrl)
    {
        var generator = new HashBasedShortCodeGenerator();
        var shortCode = generator.Generate(originalUrl);

        _output.WriteLine($"shortCode for {originalUrl} is {shortCode}");

        shortCode.Should().NotBeNull();
        shortCode.Length.Should().Be(16);
    }

    [Fact]
    public void Generate_ShouldReturnDifferentShortCodesForDifferentInputUrls()
    {
        string firstOriginalUrl = "https://www.example.com";
        string secondOriginalUrl = "https://anotherexample.com";
        var firstOriginalUrl_shortCode = _generator.Generate(firstOriginalUrl);
        var secondOriginalUrl_shortCode = _generator.Generate(secondOriginalUrl);

        _output.WriteLine($"shortCode for {firstOriginalUrl} is {firstOriginalUrl_shortCode}");
        _output.WriteLine($"shortCode for {secondOriginalUrl} is {secondOriginalUrl_shortCode}");

        firstOriginalUrl_shortCode.Should().NotBe(secondOriginalUrl_shortCode);
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
