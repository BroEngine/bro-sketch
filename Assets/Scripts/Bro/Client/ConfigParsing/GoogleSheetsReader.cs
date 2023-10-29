using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Bro.Client.ConfigParsing
{
    public class GoogleSheetsReader : IDisposable
    {
        private string _spreadsheetId;
        private string _credentialsFile;
        private TextAsset _credentialsAsset;

        private GoogleSheetsReader(string spreadsheetId) : this(spreadsheetId, GoogleSheetsSettings.Instance.Credentials) { }

        private GoogleSheetsReader(string spreadsheetId, string credentials)
        {
            _spreadsheetId = spreadsheetId;
            _credentialsFile = credentials;
        }
        
        private GoogleSheetsReader(string spreadsheetId, TextAsset credentials)
        {
            _spreadsheetId = spreadsheetId;
            _credentialsAsset = credentials;
        }


        public IEnumerable<(string title, IList<IList<object>> sheetData)> LoadAllSheets()
        {
            var sheetsService = string.IsNullOrEmpty(_credentialsFile) ? CreateSheetsService(_credentialsAsset) : CreateSheetsService(_credentialsFile);
            var spreadsheet = sheetsService.Spreadsheets.Get(_spreadsheetId).Execute();
            var sheetCount = spreadsheet.Sheets.Count;
            var processed = 0;
            var progressTitle = $"Total sheet count: {sheetCount}";
        
            try
            {
                foreach (var sheet in spreadsheet.Sheets)
                {
                    var title = sheet.Properties.Title;
                    #if UNITY_EDITOR
                    EditorUtility.DisplayProgressBar(progressTitle, $"Processing {title}", processed++ / (float)sheetCount);
                    #endif
                    if (title.StartsWith("*")) // for tables that shouldn't be processed
                    {
                        continue;
                    }
                    yield return (title, LoadSheet(sheetsService, title));
                }
            }
            finally
            {
                #if UNITY_EDITOR
                EditorUtility.ClearProgressBar();
                #endif
                sheetsService.Dispose();
                Dispose();
            }
        }
        
        private IList<IList<object>> LoadSheet(SheetsService sheetsService, string sheetTitle)
        {
            var sheetRequest = sheetsService.Spreadsheets.Values.Get(_spreadsheetId, sheetTitle);
            sheetRequest.MajorDimension = SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.ROWS;
            return sheetRequest.Execute().Values;
        }
        
        public static GoogleSheetsReader Create(string spreadsheetID)
        {
            return new GoogleSheetsReader(spreadsheetID);
        }
        
        public static GoogleSheetsReader Create(string spreadsheetID, string credentials)
        {
            return new GoogleSheetsReader(spreadsheetID, credentials);
        } 
        
        public static GoogleSheetsReader Create(string spreadsheetID, TextAsset credentials)
        {
            return new GoogleSheetsReader(spreadsheetID, credentials);
        }
        
        private SheetsService CreateSheetsService(string credentials)
        {
            var json = File.ReadAllText(credentials);
            var serviceAccountEmail = JObject.Parse(json)["client_email"].Value<string>();
            var credential = (ServiceAccountCredential)GoogleCredential.FromJson(json).UnderlyingCredential;
            var initializer = new ServiceAccountCredential.Initializer(credential.Id)
            {
                User = serviceAccountEmail,
                Key = credential.Key,
                Scopes = new[] { SheetsService.Scope.Spreadsheets }
            };
            credential = new ServiceAccountCredential(initializer);
            return new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
            });
        }
        
        private SheetsService CreateSheetsService(TextAsset credentials)
        {
            var json = credentials.text;
            var serviceAccountEmail = JObject.Parse(json)["client_email"].Value<string>();
            var credential = (ServiceAccountCredential)GoogleCredential.FromJson(json).UnderlyingCredential;
            var initializer = new ServiceAccountCredential.Initializer(credential.Id)
            {
                User = serviceAccountEmail,
                Key = credential.Key,
                Scopes = new[] { SheetsService.Scope.Spreadsheets }
            };
            credential = new ServiceAccountCredential(initializer);
            return new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
            });
        }

        public void Dispose()
        {
            //_spreadsheetId = default;
            //_credentials = default;
        }
    }
}