using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Bro.Client.ConfigParsing
{
    public class MultipleConfigParser : ISheetParser
    {
        [Serializable]
        protected class ConfigModel
        {
            [JsonProperty("default_config")]public string DefaultConfig;
            [JsonProperty("data")] public Dictionary<string, Dictionary<string,object>> Data = new Dictionary<string, Dictionary<string,object>>();
        }

        public string SheetsToJsonString(IEnumerable<(string, IList<IList<object>>)> sheets, string configTypeName, string sheetName, string previousConfig)
        {
            if (sheetName == null)
            {
                Debug.LogError("multiple parser :: sheetName is null");
                return null;
            }
            foreach (var sheet in sheets)
            {
                if (sheet.Item1.Equals(sheetName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return TableToJsonString(sheet.Item2, configTypeName, previousConfig);
                }
            }
            Debug.LogError($"multiple parser :: {sheetName} not found");
            return null;
        }

        private string TableToJsonString(IList<IList<object>> table, string typeName, string previousConfig)
        {
            bool hasInlineType = false;
            var type = ConfigParser.GetTargetType(typeName);
            if (type == null)
            {
                return null;
            }

            for (int i = 1; i < table.Count; i++) // adding empty column to first row for dynamic types
            {
                if (table[i].Count > table[0].Count)
                {
                    for (int j = 0, jMax = table[i].Count - table[0].Count; j < jMax; j++)
                    {
                        table[0].Add(new Tuple<FieldInfo, int>(null, table[0].Count));
                    }
                }
            }

            List<Tuple<FieldInfo, int>> objectFields = new List<Tuple<FieldInfo, int>>();
            // search for fields from table
            if ((string) table[0][1] == "__type")
            {
                hasInlineType = true;
            }
            else
            {
                objectFields = GetFields(table, type);
            }
            
            var model = new ConfigModel()
            {
                Data =  new Dictionary<string, Dictionary<string,object>>()
            };
            
            if (previousConfig != null)
            {
                model = JsonConvert.DeserializeObject<ConfigModel>(previousConfig);
            }

            var dict = model.Data;
            string defaultKey = null;
            for (var i = 1; i < table.Count; i++)
            {
                var firstElement = (string)table[i][0];
                if (firstElement.Contains("#"))
                {
                    continue;
                }

                var itemProperties = new Dictionary<string, object>();
                if (hasInlineType)
                {
                    var customTypeString = (string) table[i][1];
                    itemProperties["__type"] = customTypeString;
                    var inlineType = ConfigParser.GetTargetType(customTypeString);
                    objectFields = GetFields(table, inlineType);
                    type = inlineType;
                }
                
                foreach (var field in objectFields)
                {
                    if (table[i].Count <= field.Item2)
                    {
                        continue;
                    }
                    if (field.Item1 == null) // add type that not represented in table's first row
                    {
                        
                        var item = table[i][field.Item2];
                        if (String.IsNullOrEmpty(item as string))
                        {
                            continue;
                        }
                        
                        var cellDataParts = ((string)item).Split(':', StringSplitOptions.RemoveEmptyEntries);

                        if (cellDataParts.Length != 2) // wrong format. must be [type_name]:[value]
                        {
                            Debug.LogError($"Wrong format for dynamic type in row {i} column {field.Item1.Name}");
                            continue;
                        }
                        var fieldName = cellDataParts[0];
                        var fieldValue = cellDataParts[1];
                        

                        var fieldInfo = GetFieldInfoByName(fieldName, type);
                        if (fieldInfo != null)
                        {
                            foreach (var parser in ConfigParser.TypeParsers)
                            {
                                var result = parser.Key.Convert(fieldValue, fieldInfo.FieldType);
                                if (result != null)
                                {
                                    itemProperties[fieldName]= result;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError($"no field {fieldName} in type {type}");
                        }

                    }
                    else // add type that is in first row
                    {
                        foreach (var parser in ConfigParser.TypeParsers.Keys)
                        {
                            var result = parser.Convert(table[i][field.Item2], field.Item1.FieldType);
                            if (result != null)
                            {
                                var fieldCustomAttributes = field.Item1.GetCustomAttributes(typeof(JsonPropertyAttribute), true);
                                var fieldAttribute = (JsonPropertyAttribute)fieldCustomAttributes[0];
                                var fieldAttributeName = fieldAttribute.PropertyName;
                                itemProperties [fieldAttributeName] = result;
                                break;
                            }
                        }
                    }
                }

                // first column in the table contains the item keys
                var key = (string)table[i][0];
                if (defaultKey == null)
                {
                    defaultKey = key;
                }

                RowFields rowFields = GetRowFields(table,i);

                if (dict.ContainsKey(key))
                {
                    Debug.Log($"Override values for {key}");
                    

                    var configElement = new Dictionary<string, object>(); //Create new dictionary and write __type in first line. That is Important to deserialize
                    if (itemProperties.ContainsKey("__type"))
                    {
                        configElement["__type"] = new JValue(itemProperties["__type"]);
                    }

                    foreach (var elem in dict[key])
                    {
                        configElement.Add(elem.Key, elem.Value);
                    }

                    foreach (var pair in itemProperties)
                    {
                        if (pair.Key == "__type")
                        {
                            continue;
                        }    

                        configElement[pair.Key] = pair.Value;
                    }

                    
                    dict[key] = configElement; //Replace config in the model. If config type is custom __type was on th top.
                    
                }
                else
                {
                    dict[key] = itemProperties;
                }

                ParseAdditionalData(dict[key], rowFields);
            }

            return SerializeToString(model);
        }

        protected virtual string SerializeToString(ConfigModel model)
        {
            var data = JsonConvert.SerializeObject(model, Formatting.Indented);
            return data;
        }
        
        private RowFields GetRowFields(IList<IList<object>> table, int rowIndex)
        {
            var result = new RowFields();
            for (int i = 0; i < table[0].Count; i++)
            {
                string columnName = table[0][i] as string;
                object value = table[rowIndex][i];
                result.Add(columnName, value);
            }

            return result;
        }

        protected virtual void ParseAdditionalData(Dictionary<string,object> dictionary, RowFields rowFields)
        {
            
        }

        private FieldInfo GetFieldInfoByName(string name, Type parentType)
        {
            FieldInfo result = null;
            var fieldInfoArray = parentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfoArray) // add fields that type has
            {
                var fieldCustomAttributes = fieldInfo.GetCustomAttributes(typeof(JsonPropertyAttribute), true);
                if (fieldCustomAttributes.Length == 0)
                {
                    continue;
                }

                var fieldAttribute = (JsonPropertyAttribute) fieldCustomAttributes[0];
                var fieldAttributeName = fieldAttribute.PropertyName;
                fieldAttributeName = Regex.Replace(fieldAttributeName, @"(\[|\])", string.Empty);
                if (fieldAttributeName.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    result = fieldInfo;
                    break;
                }
            }

            return result;
        }

        private static List<Tuple<FieldInfo, int>> GetFields(IList<IList<object>> table, Type type)
        {
            var fields = new List<Tuple<FieldInfo, int>>();
            var fieldInfoArray = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfoArray) // add fields that type has
            {
                var fieldCustomAttributes = fieldInfo.GetCustomAttributes(typeof(JsonPropertyAttribute), true);
                if (fieldCustomAttributes.Length == 0)
                {
                    continue;
                }

                var fieldAttribute = (JsonPropertyAttribute) fieldCustomAttributes[0];
                var fieldAttributeName = fieldAttribute.PropertyName;
                fieldAttributeName = Regex.Replace(fieldAttributeName, @"(\[|\])", string.Empty);


                
                for (int i = 0; i < table[0].Count; i++)
                {
                    string columnName = table[0][i] as string;
                    if (fieldAttributeName.Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        fields.Add(Tuple.Create(fieldInfo, i));
                    }
                }
            }

            for (int i = 1; i < table[0].Count; i++) // add additional fields
            {
                if (String.IsNullOrEmpty((table[0][i] as string)))
                {
                    fields.Add(Tuple.Create<FieldInfo, int>(null, i));
                }
            }

            return fields;
        }
    }
}