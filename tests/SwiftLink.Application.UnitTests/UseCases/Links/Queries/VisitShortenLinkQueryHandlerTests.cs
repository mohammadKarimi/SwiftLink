using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftLink.Application.UnitTests.UseCases.Links.Queries;
using System;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.Notifications;
using SwiftLink.Application.UseCases.Links.Queries;
using SwiftLink.Domain.Entities;
using Xunit;

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

        // Mocking DbContext
        dbContextMock.Set<Link>().FirstOrDefaultAsync(Arg.Any<Expression<Func<Link, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(link);

        // Mocking CacheProvider
        cacheProviderMock.Get(Arg.Any<string>()).Returns(JsonSerializer.Serialize(link));

        // Mocking Mediator
        mediatorMock.WhenForAnyArgs(x => x.Publish(Arg.Any<VisitLinkNotification>(), Arg.Any<CancellationToken>()))
            .Do(x => { /* Some action */ });

        var handler = new VisitShortenLinkQueryHandler(dbContextMock, cacheProviderMock, mediatorMock);
        var query = new VisitShortenLinkQuery { ShortCode = "abc123" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeSuccessful();
        result.Value.Should().Be(link.OriginalUrl);

        // Verify that Mediator.Publish was called
        mediatorMock.Received(1).Publish(Arg.Any<VisitLinkNotification>(), Arg.Any<CancellationToken>());
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
        result.Should().BeFailure();
        result.Error.Should().Be(LinkMessages.LinkIsBanned);
    }
}
