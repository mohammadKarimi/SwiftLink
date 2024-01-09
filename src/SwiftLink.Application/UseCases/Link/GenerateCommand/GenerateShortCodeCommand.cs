namespace SwiftLink.Application.UseCases.Link.GenerateCommand;

public record GenerateShortCodeCommand
{
    public string Url { get; set; }
    public string Description { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Password { get; set; }
}
