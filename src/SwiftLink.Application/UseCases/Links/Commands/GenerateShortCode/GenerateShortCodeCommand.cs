using SwiftLink.Application.Dtos;

namespace SwiftLink.Application.UseCases.Links.Commands;

public record GenerateShortCodeCommand : IRequest<Result<LinksDto>>
{
    public string Url { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string Password { get; set; }
    public IReadOnlyList<Tags> Tags { get; set; }
}