using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.Notifications;
using SwiftLink.Application.UseCases.Links.Queries;
using SwiftLink.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SwiftLink.Application.UnitTests.UseCases.Links.Queries;

public class VisitShortenLinkQueryHandlerTests
{
    [Fact]
    public async Task Handle_ValidRequest_ReturnsOriginalUrl()
    {
        // Arrange
        var dbContextMock = Substitute.For<IApplicationDbContext>();
        var cacheProviderMock = Substitute.For<ICacheProvider>();
        var mediatorMock = Substitute.For<IMediator>();

        var link = new Link
        {
            ShortCode = "abc123",
            OriginalUrl = "https://example.com",
            IsBanned = false,
            ExpirationDate = DateTime.Now.AddDays(1),
            Password = null
        };


        dbContextMock.Set<Link>().FirstOrDefaultAsync(Arg.Any<Expression<Func<Link, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(link);


        cacheProviderMock.Get(Arg.Any<string>()).Returns(JsonSerializer.Serialize(link));

        mediatorMock.WhenForAnyArgs(x => x.Publish(Arg.Any<VisitLinkNotification>(), Arg.Any<CancellationToken>()))
            .Do(x =>
            {

            });

        var handler = new VisitShortenLinkQueryHandler(dbContextMock, cacheProviderMock, mediatorMock);
        var query = new VisitShortenLinkQuery { ShortCode = "abc123" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(true);
        result.Data.Should().Be(link.OriginalUrl);

    }

    [Fact]
    public async Task Handle_LinkIsBanned_ReturnsFailure()
    {
        // Arrange
        var dbContextMock = Substitute.For<IApplicationDbContext>();
        var cacheProviderMock = Substitute.For<ICacheProvider>();
        var mediatorMock = Substitute.For<IMediator>();

        var bannedLink = new Link
        {
            ShortCode = "banned123",
            IsBanned = true
        };

        // Mocking DbContext
        dbContextMock.Set<Link>().FirstOrDefaultAsync(Arg.Any<Expression<Func<Link, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(bannedLink);

        var handler = new VisitShortenLinkQueryHandler(dbContextMock, cacheProviderMock, mediatorMock);
        var query = new VisitShortenLinkQuery { ShortCode = "banned123" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(false);
        // result.Error.Should().Be(LinkMessages.LinkIsBanned);
    }
}
