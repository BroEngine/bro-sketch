using System;

namespace Bro.Client
{
    public class AssetSettingsAttribute : Attribute
    {
        public readonly string Name;
        public readonly string Path;

        public AssetSettingsAttribute(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}