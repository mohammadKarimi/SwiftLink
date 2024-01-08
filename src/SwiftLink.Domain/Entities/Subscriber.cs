namespace SwiftLink.Domain.Entities;

/// <summary>
/// Only these subscribers can insert a Url to get shorter one.
/// </summary>
public class Subscriber
{
    public int Id { get; set; }
    public Guid Token { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}
