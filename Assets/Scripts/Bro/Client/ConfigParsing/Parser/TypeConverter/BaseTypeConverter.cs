using System;

namespace Bro.Client.ConfigParsing
{
    public class BaseTypeConverter : ITypeConverter
    {
        public object Convert(object objToConvert, Type typeToConvertTo)
        {
            if (objToConvert == null)
            {
                return null;
            }

            try
            {
                return System.Convert.ChangeType(objToConvert, typeToConvertTo);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Can't convert object {objToConvert} to {typeToConvertTo}. Details:\n{e}");
                return null;
            }
        }
    }
}
