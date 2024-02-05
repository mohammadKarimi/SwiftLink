using System.ComponentModel;
using System.Reflection;

namespace SwiftLink.Shared;
public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());

        if (field is not null)
        {
            DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            if (attribute is not null)
                return attribute.Description;
        }
        return value.ToString();
    }
}
