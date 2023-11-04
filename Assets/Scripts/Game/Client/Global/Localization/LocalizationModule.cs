using System.Linq;
using Bro.Client;
using Bro.Client.Configs;
using Bro.Client.Context;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Client
{
    [RequireElementInContext(typeof(ConfigModule))]
    public class LocalizationModule : IClientContextModule
    {
        private ConfigModule _configModule;
        private LocalizationConfigStorage _localizationConfigStorage;
        
        public Language Language => SettingsLanguage;

        void IClientContextElement.Setup(IClientContext context)
        {
            _configModule = context.Get<ConfigModule>();
        }

        async UniTask IClientContextModule.Load()
        {
            if (SettingsLanguage == Language.Unknown)
            {
                SettingsLanguage = LanguageTool.SystemLanguage;
            }

            SettingsLanguage = ValidateLanguage(SettingsLanguage);
            
            await SwitchLanguage(SettingsLanguage);
        }

        async UniTask IClientContextModule.Unload()
        {
            
        }

        private Language ValidateLanguage(Language language)
        {
            if (ConfigPath.AvailableLanguages.Contains(language))
            {
                return language;
            }
            return Language.English;
        }


        private LocalizationConfigStorage GetLocalizationConfigStorage(Language language)
        {
            var targetIdentifier = ConfigPath.GetLocalizationIdentifier(language);
            return (LocalizationConfigStorage)_configModule.GetConfigStorage((storage)=> storage.Identifier == targetIdentifier);
        }

        public async UniTask SwitchLanguage(Language language)
        {
            var localizationStorage = GetLocalizationConfigStorage(language);
            var needToLoadStorage = localizationStorage == null;
            if (needToLoadStorage)
            {
                await _configModule.LoadConfigAsync(new ConfigStorageDescription()
                {
                    Creator = () => new LocalizationConfigStorage(),
                    Identifier = ConfigPath.GetLocalizationIdentifier(language),
                    LocalStoragePath = ConfigPath.GetLocalizationConfigPath(language),
                });
            }
            _localizationConfigStorage = GetLocalizationConfigStorage(language);
            if (_localizationConfigStorage == null)
            {
                Debug.LogError($"localization module :: cannot load config for language = {language}");
            }
            
            SettingsLanguage = language;
            new SwitchLanguageEvent(language).Launch();
            
        }

        public string Translate(string key, string[] arguments = null)
        {
            return _localizationConfigStorage.Translate(key, arguments);
        }
        
        private static Language SettingsLanguage
        {
            get => (Language) PlayerPrefs.GetInt("language", (int) Language.Unknown);
            set => PlayerPrefs.SetInt("language", (int)value);
        }
    }
}