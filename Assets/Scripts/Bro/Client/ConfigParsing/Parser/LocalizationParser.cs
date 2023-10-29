using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Bro.Client.ConfigParsing
{
    public class LocalizationParser : ISheetParser
    {
        
        [Serializable]
        private class LocalizationModel
        {
            [JsonProperty("data")] public Dictionary<string, string> Data = new Dictionary<string, string>();
        }
        
        public string SheetsToJsonString(IEnumerable<(string, IList<IList<object>>)> sheets, string configTypeName, string targetSheetName, string previousConfig)
        {
            var storageData = new Dictionary<string, Dictionary<string, string>>();
            var isSingleSheet = sheets.Count() == 1;

            foreach (var (sheetName, sheetData) in sheets)
            {
                var columnNames = sheetData[0].Cast<string>().ToList();
                for (int i = 1, max = columnNames.Count; i < max; i++)
                {
                    if (!storageData.TryGetValue(columnNames[i], out var config))
                    {
                        storageData.Add(columnNames[i], new Dictionary<string, string>());
                    }
                }
                
                for (int i = 1, max = sheetData.Count; i < max; i++)
                {
                    var row = sheetData[i];
                    var id = row[0];
                    var key = isSingleSheet ? string.Format($"{id}") : string.Format($"{sheetName}_{id}");
                    
                    for (int j = 1, subMax = row.Count; j < subMax; j++)
                    {
                        var language = columnNames[j].ToLower();

                        if (!storageData.ContainsKey(language))
                        {
                            storageData.Add(language, new Dictionary<string, string>());
                        }

                        storageData[language].Add($"[{key}]", row[j].ToString());
                    }
                }
            }

            if (storageData.TryGetValue(configTypeName, out var localizationConfig))
            {
                var localizationModel = new LocalizationModel()
                {
                    Data = localizationConfig
                };
                return JsonConvert.SerializeObject(localizationModel, Formatting.Indented);
            }
            else
            {
                Debug.LogError($"no language for type {configTypeName}");
                return string.Empty;
            }
        }
    }
}
