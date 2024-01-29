using MediatR;
using SwiftLink.Application.Dtos;

namespace SwiftLink.Application.UseCases.Links.Commands.GenerateShortCode;

public record GenerateShortCodeCommand : IRequest<Result<LinksDto>>
{
    public string Url { get; set; }
    public string Description { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string Password { get; set; }
}

public record GenerateShortCodeDto
{
    public string Url { get; set; }
    public string Description { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool IsBanned { get; set; }
}