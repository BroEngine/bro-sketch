using System;

namespace Bro.Client.ConfigParsing
{
    public class BoolTypeConverter : ITypeConverter
    {
        public object Convert(object objToConvert, Type typeToConvertTo)
        {
            if (objToConvert == null || typeToConvertTo != typeof(bool))
            {
                return null;
            }

            try
            {
                return System.Convert.ToBoolean(objToConvert);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Can't convert object {objToConvert} to {typeToConvertTo}. Details:\n{e}");
                return null;
            }
        }
    }
}
