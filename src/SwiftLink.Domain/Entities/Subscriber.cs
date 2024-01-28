namespace SwiftLink.Domain.Entities;

/// <summary>
/// Only these subscribers are allowed to insert a URL to obtain a shorter one.
/// </summary>
[Entity]
public class Subscriber : IEntity
{
    public int Id { get; set; }
    public Guid Token { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}