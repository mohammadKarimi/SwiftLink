using MediatR;

namespace SwiftLink.Application.UseCases.Links.Commands.GenerateShortCode;

public record GenerateShortCodeCommand : IRequest<Result<object>>
{
    public string Url { get; set; }
    public string Description { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string Password { get; set; }
    public Guid Token { get; set; }
}