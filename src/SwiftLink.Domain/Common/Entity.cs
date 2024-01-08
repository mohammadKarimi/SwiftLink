namespace SwiftLink.Domain.Common;

/// <summary>
/// This class is Entity Marker to find and register entities dynamically.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class EntityAttribute : Attribute
{
}
