using System;

namespace Bro.Client.ConfigParsing
{
    public class StringTypeConverter : ITypeConverter
    {
        public object Convert(object objToConvert, Type typeToConvertTo)
        {
            if (objToConvert == null || typeToConvertTo != typeof(string))
            {
                return null;
            }

            try
            {
                return System.Convert.ToString(objToConvert);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Can't convert object {objToConvert} to {typeToConvertTo}. Details:\n{e}");
                return null;
            }
        }
    }
}
