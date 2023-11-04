using Bro.Client;
using Bro.Client.Configs;
using UnityEngine;

namespace Game.Client
{
    public static class ConfigPath   // todo make it to look nice
    {
        public static readonly Language[] AvailableLanguages = new[] {Language.English}; // todo replace
        
        private static string _mainFolder => "Configs";  
        
        public static string GetLocalizationConfigPath(Language language)
        {
            return $"{_mainFolder}/config_localization_{language.GetIsoCode()}";
        }
        
        public static string GetLocalizationIdentifier(Language language)
        {
            return $"config_localization_{language.GetIsoCode()}";
        }
        
        public static string GetConfigPath<T>() where T : IConfigStorage
        {
            return $"{_mainFolder}/{GetConfigName<T>()}";
        }
        
        public static string GetConfigName<T>() where T : IConfigStorage
        {
            return $"config_{GetIdentifier<T>()}";
        }
        
        public static string GetIdentifier<T>() where T : IConfigStorage
        {
            var type = typeof(T);
            
            if (type == typeof(VehicleConfigStorage))
            {
                return "vehicles";
            }
            
            Debug.LogError($"config registry : cannot find identifier for type {type}");
            return "";
        }
    }
}