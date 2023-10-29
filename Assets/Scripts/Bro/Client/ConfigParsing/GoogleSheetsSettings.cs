using System;
using System.Collections.Generic;
using Bro.Client;
using UnityEditor;

namespace Bro.Client.ConfigParsing
{
    [AssetSettings("settings_google_sheets", "Resources/Settings")]
    public class GoogleSheetsSettings : AssetSettings<GoogleSheetsSettings>
    {
        #if UNITY_EDITOR
        [MenuItem("Settings/Google Sheets Settings")]
        private static void OpenSettings()
        {
            Instance = null;
            Selection.activeObject = Instance;
            DirtyEditor();
        }
        #endif
        
        
        [Serializable]
        public class ConfigDescription
        {
            public string TypeNameAttribute;
            public string ConfigFileName;
            public string SheetName;
            public string DownloadSpreadsheetId;
            public ParserType ParserType;
            public ParseMode ParseMode = ParseMode.MakeNewConfig;
            public string CustomParserName;
        }

        public List<ConfigDescription> Configs;

        public string Credentials;
        public string ConfigsFolderPath;
    }
}
