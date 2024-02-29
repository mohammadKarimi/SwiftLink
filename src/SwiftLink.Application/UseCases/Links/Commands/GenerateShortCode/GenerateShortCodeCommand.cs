using SwiftLink.Application.Dtos;
using SwiftLink.Domain.Entities.Link.ValueObject;

namespace SwiftLink.Application.UseCases.Links.Commands;

public record GenerateShortCodeCommand : IRequest<Result<LinksDto>>
{
    public string GroupName { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? RemindDate { get; set; }
    public string Password { get; set; }
    public string BackHalf { get; set; }
    public IReadOnlyList<Tag> Tags { get; set; }
}
