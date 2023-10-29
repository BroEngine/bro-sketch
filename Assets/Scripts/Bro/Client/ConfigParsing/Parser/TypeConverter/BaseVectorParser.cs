using System;
using System.Collections.Generic;

namespace Bro.Client.ConfigParsing
{
    public class BaseVectorParser
    {
        protected List<float> ParseFloatCoordinates(object objToConvert)
        {
            var items = ((string) objToConvert).Split(new char[] {'{', ' ', '}'}, StringSplitOptions.RemoveEmptyEntries);
            var coordinates = new List<float>();
            foreach (var item in items)
            {
                var floatParser = new FloatTypeConverter();
                var result = floatParser.Convert(item, typeof(float));
                if (result != null)
                {
                    coordinates.Add((float) result);
                }
            }

            return coordinates;
        }
    }
}