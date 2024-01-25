using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.Common;

public class SharedContext : Dictionary<string, object>, ISharedContext
{
    public object Get(string key)
    {
        TryGetValue(key, out var value);
        return value;
    }

    public void Set(string key, object value)
        => Add(key, value);
}