namespace SwiftLink.Domain.Entities;

public class Tag : IEquatable<Tag>
{
    public string Title { get; private set; }
    public int Order { get; private set; }

    private Tag(string title, int order)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tag title cannot be empty.", nameof(title));

        Title = title;
        Order = order;
    }

    public static Tag Create(string title, int order)
        => new(title, order);

    public bool Equals(Tag other)
        => other is not null && (ReferenceEquals(this, other) || Title == other.Title && Order == other.Order);

    public override bool Equals(object obj)
        => obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Tag)obj));

    public override int GetHashCode()
        => HashCode.Combine(Title, Order);

    public static bool operator ==(Tag left, Tag right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Tag left, Tag right)
    {
        return !Equals(left, right);
    }
}
