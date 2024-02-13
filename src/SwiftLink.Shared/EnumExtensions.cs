using System.ComponentModel;

namespace SwiftLink.Shared;
public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        if (field is not null)
        {
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            if (attribute is not null)
                return attribute.Description;
        }
        return value.ToString();
    }
}
