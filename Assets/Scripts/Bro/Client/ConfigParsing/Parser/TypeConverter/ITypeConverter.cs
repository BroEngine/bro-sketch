using System;

namespace Bro.Client.ConfigParsing
{
    public interface ITypeConverter
    {
        public object Convert(object objToConvert, Type typeToConvertTo);
    }
}
