using System.Linq.Expressions;
using Azure.Core;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.UseCases.Links.Queries;
using SwiftLink.Domain.Entities.Link;
using Xunit;

namespace SwiftLink.Application.UnitTests.UseCases.Links.Queries;

public class VisitShortenLinkQueryHandlerTests
{

    private readonly IApplicationDbContext _dbContextMock;
    private readonly IMediator _mediatorMock;
    private readonly VisitShortenLinkQueryHandler _handler;
    private readonly ICacheProvider _cacheProviderMock;

    public VisitShortenLinkQueryHandlerTests()
    {
        _dbContextMock = Substitute.For<IApplicationDbContext>();
        _cacheProviderMock = Substitute.For<ICacheProvider>();
        _mediatorMock = Substitute.For<IMediator>();

        _handler = new VisitShortenLinkQueryHandler(
            _dbContextMock,
            _cacheProviderMock,
           _mediatorMock
        );
    }

    //[Fact]
    //public async Task Handle_ValidRequest_ReturnsOriginalUrl()
    //{
    //    // Arrange
    //    var dbContextMock = Substitute.For<IApplicationDbContext>();
    //    var cacheProviderMock = Substitute.For<ICacheProvider>();
    //    var mediatorMock = Substitute.For<IMediator>();

    //    var link = new Link
    //    {
    //        ShortCode = "abc123",
    //        OriginalUrl = "https://example.com",
    //        IsBanned = false,
    //        ExpirationDate = DateTime.Now.AddDays(1),
    //        Password = null
    //    };


    //    dbContextMock.Set<Link>().FirstOrDefaultAsync(Arg.Any<Expression<Func<Link, bool>>>(), Arg.Any<CancellationToken>())
    //        .Returns(link);


    //    cacheProviderMock.Get(Arg.Any<string>()).Returns(JsonSerializer.Serialize(link));

    //    mediatorMock.WhenForAnyArgs(x => x.Publish(Arg.Any<VisitLinkNotification>(), Arg.Any<CancellationToken>()))
    //        .Do(x =>
    //        {

    //        });

    //    var handler = new VisitShortenLinkQueryHandler(dbContextMock, cacheProviderMock, mediatorMock);
    //    var query = new VisitShortenLinkQuery { ShortCode = "abc123" };

    //    // Act
    //    var result = await handler.Handle(query, CancellationToken.None);

    //    // Assert
    //    result.IsSuccess.Should().Be(true);
    //    result.Data.Should().Be(link.OriginalUrl);

    //}

    //[Fact]
    //public async Task Handle_LinkIsBanned_ReturnsFailure()
    //{
    //    // Arrange
    //    var dbContextMock = Substitute.For<IApplicationDbContext>();
    //    var cacheProviderMock = Substitute.For<ICacheProvider>();
    //    var mediatorMock = Substitute.For<IMediator>();

    //    var bannedLink = new Link
    //    {
    //        ShortCode = "banned123",
    //        IsBanned = true
    //    };

    //    // Mocking DbContext
    //    dbContextMock.Set<Link>().FirstOrDefaultAsync(Arg.Any<Expression<Func<Link, bool>>>(), Arg.Any<CancellationToken>())
    //        .Returns(bannedLink);

    //    var handler = new VisitShortenLinkQueryHandler(dbContextMock, cacheProviderMock, mediatorMock);
    //    var query = new VisitShortenLinkQuery { ShortCode = "banned123" };

    //    // Act
    //    var result = await handler.Handle(query, CancellationToken.None);

    //    // Assert
    //    result.IsSuccess.Should().Be(false);
    //    // result.Error.Should().Be(LinkMessages.LinkIsBanned);
    //}


    [Fact]
    public async Task Handle_LinkNotFound_ReturnsFailure()
    {
        _cacheProviderMock.Get(Arg.Any<string>()).Returns((string)null);
        _dbContextMock.Set<Link>().ReturnsForAnyArgs(Substitute.For<DbSet<Link>>());
        _dbContextMock.Set<Link>().FirstOrDefaultAsync(Arg.Any<Expression<Func<Link, bool>>>(), Arg.Any<CancellationToken>())
              .ReturnsNull();
        var query = new VisitShortenLinkQuery { ShortCode = "nonexistent123" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(false);
    }

    [Fact]
    public async Task Handle_LinkExpired_ReturnsFailure()
    {
        var expiredLink = new Link
        {
            ShortCode = "expired123",
            ExpirationDate = DateTime.Now.AddDays(-1)
        };

        _dbContextMock.Set<Link>().FirstOrDefaultAsync(Arg.Any<Expression<Func<Link, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(expiredLink);

        // Act
        var result = await _handler.Handle(new VisitShortenLinkQuery { ShortCode = "expired123" }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(false);
    }

    [Fact]
    public async Task Handle_LinkRequiresPassword_ReturnsFailureOnIncorrectPassword()
    {
        var passwordProtectedLink = new Link
        {
            ShortCode = "protected123",
            Password = "correctPassword" // Assuming some hashing in real scenario
        };

        _dbContextMock.Set<Link>().FirstOrDefaultAsync(Arg.Any<Expression<Func<Link, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(passwordProtectedLink);

        var query = new VisitShortenLinkQuery { ShortCode = "protected123", Password = "wrongPassword" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(false);
    }
}
