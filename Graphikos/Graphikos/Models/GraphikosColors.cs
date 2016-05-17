using System;
using System.ComponentModel;

namespace Graphikos.Models
{
    public enum GraphikosColors
    {
        [Description("#000000")]
        Black,
        [Description("#ff0000")]
        Red
    }

    public static class EnumDescriptions
    {
        public static string GetEnumDescription(Enum enumValue)
        {
            var enumValueAsString = enumValue.ToString();

            var type = enumValue.GetType();
            var fieldInfo = type.GetField(enumValueAsString);
            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length <= 0) return enumValueAsString;
            var attribute = (DescriptionAttribute)attributes[0];
            return attribute.Description;
        }
    }
}
