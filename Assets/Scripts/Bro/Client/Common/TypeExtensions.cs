using System;
using System.Collections.Generic;

namespace Bro.Client
{
    public static class TypeExtensions
    {
        public static List<Type> GetHierarchy(this Type type, bool exceptObjectType = true)
        {
            var result = new List<Type>();
            while (type != null)
            {
                if (type == typeof(Object))
                {
                    if (!exceptObjectType)
                    {
                        result.Add(type);
                    }
                }
                else
                {
                    result.Add(type);
                }

                type = type.BaseType;
            }

            return result;
        }
    }
}