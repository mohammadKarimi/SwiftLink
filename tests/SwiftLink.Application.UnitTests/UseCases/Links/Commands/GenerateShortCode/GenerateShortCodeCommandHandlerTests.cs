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
    private readonly GenerateShortCodeCommandHandler _handler;

    public GenerateShortCodeCommandHandlerTests()
    {
        _dbContextMock = Substitute.For<IApplicationDbContext>();
        _cacheProviderMock = Substitute.For<ICacheProvider>();
        _codeGeneratorMock = Substitute.For<IShortCodeGenerator>();
        _optionsMock = Substitute.For<IOptions<AppSettings>>();
        _sharedContextMock = Substitute.For<ISharedContext>();

        _handler = new GenerateShortCodeCommandHandler(
            _dbContextMock,
            _cacheProviderMock,
            _codeGeneratorMock,
            _optionsMock,
            _sharedContextMock
        );
    }

    private async Task<Result<LinksDto>> ExecuteHandler(GenerateShortCodeCommand request, Link link)
    {
        _dbContextMock.Set<Link>().ReturnsForAnyArgs(Substitute.For<DbSet<Link>>());
        _codeGeneratorMock.Generate(request.Url).Returns(link.ShortCode);
        _sharedContextMock.Get(nameof(Subscriber.Id)).Returns(1);
        return await _handler.Handle(request, CancellationToken.None);
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
        };

        var link = new Link
        {
            OriginalUrl = request.Url,
            ShortCode = "generatedShortCode",
            Description = request.Description,
            SubscriberId = 1,
            ExpirationDate = request.ExpirationDate ?? DateTime.Now.AddDays(7),
            IsBanned = false
        };

        _dbContextMock.SaveChangesAsync(CancellationToken.None).Returns(Result.Success());

        // Act
        var result = await ExecuteHandler(request, link);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.ExpirationDate.Should().Be(link.ExpirationDate);
        result.Data.IsBanned.Should().Be(link.IsBanned);
        result.Data.ShortCode.Should().Be(link.ShortCode);
        result.Data.OriginalUrl.Should().Be(link.OriginalUrl);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCanNotInsertToDb()
    {
        // Arrange
        var request = new GenerateShortCodeCommand
        {
            Url = "https://www.example.com",
            Description = "Sample description",
            ExpirationDate = DateTime.UtcNow.AddDays(7)
        };

        var link = new Link
        {
            OriginalUrl = request.Url,
            ShortCode = "generatedShortCode",
            Description = request.Description,
            SubscriberId = 1,
            ExpirationDate = request.ExpirationDate ?? DateTime.Now.AddDays(7),
            IsBanned = false
        };

        _dbContextMock.SaveChangesAsync(CancellationToken.None).Returns(Result.Failure(Error.None));

        // Act
        var result = await ExecuteHandler(request, link);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(CommonMessages.Database.InsertFailed);
    }

    [Fact]
    public async Task Handle_ShouldSetDefaultExpirationTime_WhenExpirationDateIsNull()
    {
        // Arrange
        var request = new GenerateShortCodeCommand
        {
            Url = "https://www.example.com",
            Description = "Sample description",
            ExpirationDate = null
        };

        var link = new Link
        {
            OriginalUrl = request.Url,
            ShortCode = "generatedShortCode",
            Description = request.Description,
            SubscriberId = 1,
            ExpirationDate = DateTime.Now.AddDays(7),
            IsBanned = false
        };

        _dbContextMock.SaveChangesAsync(CancellationToken.None).Returns(Result.Success());
        _optionsMock.Value.Returns(new AppSettings { DefaultExpirationTimeInDays = 7 });

        // Act
        var result = await ExecuteHandler(request, link);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.ExpirationDate.Date.Should().Be(link.ExpirationDate.Date);
        result.Data.ExpirationDate.Hour.Should().Be(link.ExpirationDate.Hour);
        result.Data.ExpirationDate.Minute.Should().Be(link.ExpirationDate.Minute);
    }
}
