using FluentAssertions;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.UnitTests;

[TestFixture]
public class HashBasedShortCodeGeneratorTests
{
    private IShortCodeGenerator _generator;
    private string _originalUrl;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _generator = new HashBasedShortCodeGenerator();
        _originalUrl = "https://example.com";
    }

    [Test]
    public void HashBasedShortCodeGenerator_ShouldGenerateShortCode()
    {
        // Act
        var shortCode = _generator.Generate(_originalUrl);

        // Assert
        shortCode.Should().NotBeNull();
        shortCode.Should().NotBeEmpty();
        shortCode.Length.Should().Be(16, "8 characters random + 8 characters hash");
    }

    [Test]
    public void HashBasedShortCodeGenerator_ShouldBeThreadSafe()
    {
        // Act
        Parallel.For(0, 10, _ =>
        {
            var shortCode = _generator.Generate(_originalUrl);
            TestContext.Out.WriteLine(shortCode);

            // Assert
            shortCode.Should().NotBeNull();
            shortCode.Should().NotBeEmpty();
            shortCode.Length.Should().Be(16, "8 characters random + 8 characters hash");
        });
    }
}