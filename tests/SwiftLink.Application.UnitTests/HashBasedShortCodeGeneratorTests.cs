using FluentAssertions;
using SwiftLink.Application.Common;

namespace SwiftLink.Application.UnitTests;

[TestFixture]
public class HashBasedShortCodeGeneratorTests
{
    private HashBasedShortCodeGenerator generator;
    private string originalUrl;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        generator = new HashBasedShortCodeGenerator();
        originalUrl = "https://example.com";
    }

    [Test]
    public void HashBasedShortCodeGenerator_ShouldGenerateShortCode()
    {
        // Act
        var shortCode = generator.Generate(originalUrl);

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
            var shortCode = generator.Generate(originalUrl);
            TestContext.Out.WriteLine(shortCode);

            // Assert
            shortCode.Should().NotBeNull();
            shortCode.Should().NotBeEmpty();
            shortCode.Length.Should().Be(16, "8 characters random + 8 characters hash");
        });
    }
}