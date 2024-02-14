namespace SwiftLink.Application.UseCases.Links.Queries;

public record InquiryBackHalfQuery(string BackHalfText) : IRequest<Result<bool>>;
