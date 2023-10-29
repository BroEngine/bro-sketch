using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Bro.Client.Json;
using UnityEngine;

namespace Bro.Client.ConfigParsing
{
    public static class ConfigParser
    {
        /// parsers for different types
        public static readonly Dictionary<ITypeConverter, Type> TypeParsers = new Dictionary<ITypeConverter, Type>
        {
            { new ListTypeConverter(), typeof(IList<object>) },
            { new Vector2Converter(), typeof(Vector2) },
            { new Vector3Converter(), typeof(Vector3) },
            { new EnumTypeConverter(), typeof(Enum) },
            { new FloatTypeConverter(), typeof(float) },
            { new BoolTypeConverter(), typeof(bool) },
            { new StringTypeConverter(), typeof(string) },
            { new BaseTypeConverter(), typeof(object) },
        };

        /// save data for certain type to json file 
        public static void Parse(IEnumerable<(string, IList<IList<object>>)> sheets, string sheetName, string configTypeName, string configFilePath, ParserType parserType, ParseMode parseMode, string customParserName)
        {
            var parser = GetParser(parserType, customParserName);
            if (parser != null)
            {
                string previousConfig = null;
                if (parseMode == ParseMode.AppendToExistingConfig)
                {
                    previousConfig = LoadFromFile(configFilePath);
                }

                var config = parser.SheetsToJsonString(sheets, configTypeName, sheetName, previousConfig);
                if (config != null)
                {
                    SaveToFile(config, configFilePath);
                }
            }
            else
            {
                Debug.LogError($"parser {parserType} not found");
            }
        }
        
        public static string Parse(IEnumerable<(string, IList<IList<object>>)> sheets, string sheetName, string configTypeName, ParserType parserType, ParseMode parseMode, string customParserName)
        {
            var parser = GetParser(parserType, customParserName);
            if (parser != null)
            {
                var config = parser.SheetsToJsonString(sheets, configTypeName, sheetName, null);
                return config;
            }
            else
            {
                Debug.LogError($"parser {parserType} not found");
            }
            return null;
        }

        private static string LoadFromFile(string configFilePath)
        {
            return File.ReadAllText(configFilePath);
        }

        /// find type in current assembly by name
        public static Type GetTargetType(string typeName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    var typeCustomAttributes = type.GetCustomAttributes(typeof(JsonTypeAttribute), true);
                    if (typeCustomAttributes.Length == 0)
                    {
                        continue;
                    }

                    var typeAttribute = (JsonTypeAttribute)typeCustomAttributes[0];
                    var typeAttributeName = typeAttribute.TypeName;
                    typeAttributeName = Regex.Replace(typeAttributeName, @"(\[|\])", string.Empty);
                    if (typeAttributeName.Equals(typeName))
                    {
                        return type;
                    }
                }
            }

            //var type = assembly.GetTypes().FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.CurrentCultureIgnoreCase));

            Debug.LogError($"Type {typeName} not found in current assembly ");
            return null;
        }

        /// save string to file
        private static void SaveToFile(string config, string configFilePath)
        {
            File.WriteAllText(configFilePath, config);
        }

        private static ISheetParser GetParser(ParserType parserType, string customParserName) =>
            parserType switch
            {
                ParserType.MultipleItems => new MultipleConfigParser(),
                ParserType.SingleItem => new SingleConfigParser(),
                ParserType.Localization => new LocalizationParser(),
                ParserType.Custom => CreateParserByClassName(customParserName),
                _ => null,
            };

        private static ISheetParser CreateParserByClassName(string parserName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.Name == parserName)
                    {
                        return (ISheetParser)System.Activator.CreateInstance(type);
                    }
                }
            }

            Debug.LogError("config parser :: cannot find custom class " + parserName);
            return null;
        }
    }
}