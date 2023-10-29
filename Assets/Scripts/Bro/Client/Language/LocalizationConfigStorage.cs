using Bro.Client.Configs;
using UnityEngine;

namespace Bro.Client
{
    public class LocalizationConfigStorage : DictionaryConfigStorage<string, string>
    {
        public string Translate(string key, string[] arguments)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            var translatedText = GetConfig(key);
            if (arguments != null && arguments.Length > 0)
            {
                // ReSharper disable once CoVariantArrayConversion
                translatedText = string.Format(translatedText, arguments);
            }

            if (translatedText == null)
            {
                Debug.LogError($"localization config storage :: no translation for key {key}");
                translatedText = key;
            }
            
            return translatedText;
        }
    }
}