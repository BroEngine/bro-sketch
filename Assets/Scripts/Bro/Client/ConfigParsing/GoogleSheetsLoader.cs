using System.Collections.Generic;
using UnityEngine;

namespace Bro.Client.ConfigParsing
{
    public static class GoogleSheetsLoader
    {
        public static IEnumerable<(string, IList<IList<object>>)> Load(string spreadsheetID, string credentials)
        {
            var googleSheetsReader = GoogleSheetsReader.Create(spreadsheetID, credentials);
            return googleSheetsReader.LoadAllSheets();
        }
        
        public static IEnumerable<(string, IList<IList<object>>)> Load(string spreadsheetID, TextAsset credentials)
        {
            var googleSheetsReader = GoogleSheetsReader.Create(spreadsheetID, credentials);
            return googleSheetsReader.LoadAllSheets();
        }
    }
}