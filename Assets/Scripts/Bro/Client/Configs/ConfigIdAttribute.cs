using System;

namespace Bro.Client.Configs
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigIdAttribute : Attribute
    {
    }
}