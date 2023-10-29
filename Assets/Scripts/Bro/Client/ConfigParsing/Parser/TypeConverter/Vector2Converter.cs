using System;
using UnityEngine;

namespace Bro.Client.ConfigParsing
{
    public class Vector2Converter : BaseVectorParser, ITypeConverter
    {
        public object Convert(object objToConvert, Type typeToConvertTo)
        {
            if (objToConvert == null || typeToConvertTo != typeof(Vector2))
            {
                return null;
            }
            var coordinates = ParseFloatCoordinates(objToConvert);

            if (coordinates.Count == 2)
            {
                return new Vector2(coordinates[0], coordinates[1]);
            }
            else
            {
                return null;
            }
        }
    }
}