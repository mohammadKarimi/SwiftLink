using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.UseCases.Links.Commands.GenerateShortCode;
using SwiftLink.Domain.Entities;
using SwiftLink.Shared;
using Xunit;

namespace SwiftLink.Application.UnitTests.UseCases.Links.Commands.GenerateShortCode;

public class GenerateShortCodeCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldInsertLinkAndSetCache_WhenRequestIsValid()
    {
        // Arrange
        var dbContextMock = Substitute.For<IApplicationDbContext>();
        var cacheProviderMock = Substitute.For<ICacheProvider>();
        var codeGeneratorMock = Substitute.For<IShortCodeGenerator>();
        var optionsMock = Substitute.For<IOptions<AppSettings>>();
        var sharedContextMock = Substitute.For<ISharedContext>();

        var handler = new GenerateShortCodeCommandHandler(
            dbContextMock,
            cacheProviderMock,
            codeGeneratorMock,
            optionsMock,
            sharedContextMock
        );

        var request = new GenerateShortCodeCommand
        {
            Url = "https://www.example.com",
            Description = "Sample description",
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            Password = "password123"
        };

        var link = new Link
        {
            OriginalUrl = request.Url,
            ShortCode = "generatedShortCode",
            Description = request.Description,
            SubscriberId = 1,
            ExpirationDate = request.ExpirationDate ?? DateTime.Now.AddDays(7),
            Password = "hashedPassword"
        };

        dbContextMock.Set<Link>().ReturnsForAnyArgs(Substitute.For<DbSet<Link>>());
        codeGeneratorMock.Generate(request.Url).Returns(link.ShortCode);
        sharedContextMock.Get(nameof(Subscriber.Id)).Returns(1);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();

        await dbContextMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        await cacheProviderMock.Received(1).Set(link.ShortCode, Arg.Any<string>(), link.ExpirationDate);

        var responseObject = result.Data;
        responseObject.Should().NotBeNull();

        responseObject.ExpirationDate.Should().Be(link.ExpirationDate);
        responseObject.IsBanned.Should().Be(link.IsBanned);
        responseObject.ShortCode.Should().Be(link.ShortCode);
        responseObject.OriginalUrl.Should().Be(link.OriginalUrl);
    }
}

