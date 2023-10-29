using System;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Bro.Client.ConfigParsing
{
    public class EnumTypeConverter : ITypeConverter
    {
        public object Convert(object objToConvert, Type typeToConvertTo)
        {
            if (!typeToConvertTo.IsEnum)
            {
                return null;
            }

            // return minimal enum value if it wasn't specified 
            if (string.IsNullOrEmpty(objToConvert as string))
            {
                var enumValues = Enum.GetValues(typeToConvertTo);
                var minIndex = 0;
                for (int j = 0; j < enumValues.Length; j++)
                {
                    var val = (int) enumValues.GetValue(j);
                    if (val < (int) enumValues.GetValue(minIndex))
                    {
                        minIndex = j;
                    }
                }
                return Enum.GetValues(typeToConvertTo).GetValue(minIndex);
            }

            try
            {
                if (Attribute.GetCustomAttribute(typeToConvertTo, typeof(JsonConverterAttribute)) == null)
                {
                    return Enum.Parse(typeToConvertTo, (string) objToConvert, true);
                }
                // else
                // {
                //     return Enum.GetValues(typeToConvertTo)
                //         .Cast<Enum>()
                //         .FirstOrDefault(v => v.GetJsonProperty() == (string)objToConvert);
                // }
                
                return Enum.Parse(typeToConvertTo, (string) objToConvert, true);

            }
            catch (Exception e)
            {
                return ToEnum(typeToConvertTo, (string) objToConvert);
                
                UnityEngine.Debug.LogError($"Can't convert object {objToConvert} to {typeToConvertTo}. Details:\n{e}");
                return null;
            }
        }
        
        private static object ToEnum(Type enumType, string str)
        {
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str) return Enum.Parse(enumType, name);
            }
            return null;
        }
    }
}