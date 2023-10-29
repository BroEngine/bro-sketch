using System;
using System.Globalization;

namespace Bro.Client.ConfigParsing
{
    public class FloatTypeConverter : ITypeConverter
    {
        public object Convert(object objToConvert, Type typeToConvertTo)
        {
            if (objToConvert == null || typeToConvertTo != typeof(float))
            {
                return null;
            }

            try
            {
                if (objToConvert is string)
                {
                    objToConvert = ((string) objToConvert).Replace(",", ".");
                }

                return System.Convert.ToSingle(objToConvert,new CultureInfo("en")
                {
                    NumberFormat =
                    {
                        NumberDecimalSeparator = "."
                    }
                });
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Can't convert object {objToConvert} to {typeToConvertTo}. Details:\n{e}");
                return null;
            }
        }
    }
}
