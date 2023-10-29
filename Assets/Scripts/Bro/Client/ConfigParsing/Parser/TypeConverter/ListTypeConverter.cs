using System;
using System.Collections.Generic;

namespace Bro.Client.ConfigParsing
{
    public class ListTypeConverter : ITypeConverter
    {
        public object Convert(object objToConvert, Type typeToConvertTo)
        {
            var isList = typeToConvertTo == typeof(IList<object>) || typeToConvertTo.IsGenericType;

            if (objToConvert == null || !isList)
            {
                return null;
            }
            var source = (string) objToConvert;

            if(!source.Contains("[") || !source.Contains("]"))
            {
                return null;
            }

            var list = new List<object>();
            var items = ((string)objToConvert).Split(new char[] { '[', ';', ']' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                foreach (var parser in ConfigParser.TypeParsers.Keys)
                {
                    if (parser is ListTypeConverter)
                    {
                        continue;
                    }

                    var result = parser.Convert(item, ConfigParser.TypeParsers[parser]);
                    if (result != null)
                    {
                        list.Add(result);
                        break;
                    }
                }
            }
            return list;
        }
    }
}
