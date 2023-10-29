using System;

namespace Bro.Client.Configs
{
    public class ConfigStorageDescription
    {
        public Func<IConfigStorage> Creator;
        public string Identifier;
        public string LocalStoragePath;
        public string PersistentStoragePath;
    }
}