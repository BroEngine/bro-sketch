using System;
using Bro.Client.ConfigParsing;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bro.Client.Configs
{
    public static class ConfigStorageUpdater
    {
        public static async UniTask UpdateStorage<T,V>(string configFileName, DictionaryConfigStorage<T,V> storage) where V : class
        {
            try
            {
                var settings = GoogleSheetsSettings.Instance;
                var credentials = settings.Credentials;
                credentials = credentials.Replace(".json",string.Empty);
                credentials = credentials.Replace("Assets/Resources/",string.Empty);
                
                foreach (var config in settings.Configs)
                {
                    if (config.ConfigFileName == configFileName)
                    {
                        Debug.LogError($"updating: [{configFileName}] with credentials at: [{credentials}]");
                        
                        var credentialAsset = Resources.Load<TextAsset>(credentials);
                        var sheets = GoogleSheetsLoader.Load(config.DownloadSpreadsheetId, credentialAsset);
                        var json = ConfigParser.Parse(sheets, config.SheetName, config.TypeNameAttribute, config.ParserType, config.ParseMode, config.CustomParserName);
                        
                        storage.Clear();
                        await storage.InitializeAsync(json, 0, storage.Identifier);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("storage updating failed" + e);
            }
        }
    }
}