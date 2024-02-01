using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;
using SwiftLink.Application.Dtos;
using SwiftLink.Application.UseCases.Links.Commands;
using SwiftLink.Domain.Entities;
using SwiftLink.Shared;
using Xunit;

namespace SwiftLink.Application.UnitTests.UseCases.Links.Commands.GenerateShortCode;
 
public class GenerateShortCodeCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContextMock;
    private readonly ICacheProvider _cacheProviderMock;
    private readonly IShortCodeGenerator _codeGeneratorMock;
    private readonly IOptions<AppSettings> _optionsMock;
    private readonly ISharedContext _sharedContextMock;
    private readonly GenerateShortCodeCommandHandler handler;

    public GenerateShortCodeCommandHandlerTests()
    {
        _dbContextMock = Substitute.For<IApplicationDbContext>();
        _cacheProviderMock = Substitute.For<ICacheProvider>();
        _codeGeneratorMock = Substitute.For<IShortCodeGenerator>();
        _optionsMock = Substitute.For<IOptions<AppSettings>>();
        _sharedContextMock = Substitute.For<ISharedContext>();

        handler = new GenerateShortCodeCommandHandler(
            _dbContextMock,
            _cacheProviderMock,
            _codeGeneratorMock,
            _optionsMock,
            _sharedContextMock
        );
    }

    [Fact]
    public async Task Handle_ShouldInsertLinkAndSetCache_WhenRequestIsValid()
    {
        // Arrange
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
            Password = "hashedPassword",
            IsBanned = false
        };

        _dbContextMock.Set<Link>().ReturnsForAnyArgs(Substitute.For<DbSet<Link>>());
        _codeGeneratorMock.Generate(request.Url).Returns(link.ShortCode);
        _sharedContextMock.Get(nameof(Subscriber.Id)).Returns(1);
        _dbContextMock.SaveChangesAsync(default).Returns(Result.Success());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();

        var responseObject = result.Data;
        responseObject.Should().NotBeNull();

        responseObject.ExpirationDate.Should().Be(link.ExpirationDate);
        responseObject.IsBanned.Should().Be(link.IsBanned);
        responseObject.ShortCode.Should().Be(link.ShortCode);
        responseObject.OriginalUrl.Should().Be(link.OriginalUrl);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCanNotInsertToDb()
    {
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
            Password = "hashedPassword",
            IsBanned = false
        };

        _dbContextMock.Set<Link>().ReturnsForAnyArgs(Substitute.For<DbSet<Link>>());
        _codeGeneratorMock.Generate(request.Url).Returns(link.ShortCode);
        _sharedContextMock.Get(nameof(Subscriber.Id)).Returns(1);
        _dbContextMock.SaveChangesAsync(default).Returns(Result.Failure(Error.None));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(CommonMessages.Database.InsertFailed);
    }
}

