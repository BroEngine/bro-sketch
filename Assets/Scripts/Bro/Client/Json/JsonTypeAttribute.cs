using System;

namespace Bro.Client.Json
{
    // attribute for config parsers 
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class JsonTypeAttribute : System.Attribute
    {
        public string TypeName { get; }

        public JsonTypeAttribute(string typeName)
        {
            TypeName = typeName;
        }
    }
}