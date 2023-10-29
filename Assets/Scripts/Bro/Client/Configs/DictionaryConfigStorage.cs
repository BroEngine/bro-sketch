using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bro.Client.Json;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Bro.Client.Configs
{
    public class DictionaryConfigStorage<K, V>: IConfigProvider<K,V>, IConfigStorage where V : class
    {
        public class CollectionModel<KModel, VModel>
        {
            [JsonProperty("default_id")] public KModel DefaultConfigId = default(KModel);
            [JsonProperty("data")] public Dictionary<KModel, VModel> Data = new Dictionary<KModel, VModel>();

            public VModel GetDefaultConfig()
            {
                if (!Data.TryGetValue(DefaultConfigId, out var result))
                {
                    result = default(VModel);
                }

                return result;
            }
        }
        
        private FieldInfo _configIdFieldInfo;
        private readonly object _sync = new object();
        private CollectionModel<K, V> _configCollection = new CollectionModel<K, V>();
        
        protected virtual JsonSerializerSettings SerializerSettings => DefaultJsonSerializerSettings.AutoSettings;
        
        public string Identifier { get; private set; }
        public int Version { get; private set; }
        
        public IReadOnlyDictionary<K, V> Data
        {
            get
            {
                lock (_sync)
                {
                    return _configCollection.Data;
                }
            }
        }

        public async UniTask InitializeAsync(string configData, int version, string identifier)
        {
            if (string.IsNullOrEmpty(configData))
            {
                Debug.LogError($"single config storage {GetType()} :: invalid config data, its empty");
                return;
            }
            
            try
            {   
                var deserializedObject = await UniTask.RunOnThreadPool(() => JsonConvert.DeserializeObject<CollectionModel<K, V>>(configData, SerializerSettings));
                lock (_sync)
                {
                    Version = version;
                    Identifier = identifier;
                    _configCollection = deserializedObject;
                    
                    OnConfigsInitialized(_configCollection);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"dictionary config storage {GetType()} :: error during parsing configs \n exception {e}\n json = {configData}");
            }
        }

        protected virtual void OnConfigsInitialized(CollectionModel<K, V> configCollection)
        {
            SetKeyInConfigs();
        }
        
        private void SetKeyInConfigs()
        {
            _configIdFieldInfo = null;
            var props = typeof(V).GetFields();
            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(true);
                if (attrs.OfType<ConfigIdAttribute>().Any())
                {
                    _configIdFieldInfo = prop;
                    break;
                }
            }

            if (_configIdFieldInfo == null)
            {
                return;
            }
            
            foreach (var config in _configCollection.Data)
            {
                _configIdFieldInfo.SetValue(config.Value, config.Key);
            }
        }

        public V GetDefaultConfig()
        {
            lock (_sync)
            {
                return _configCollection.GetDefaultConfig();
            }
        }

        public V GetConfig(K configId)
        {
            lock (_sync)
            {
                if (_configCollection.Data.TryGetValue(configId, out var result))
                {
                    return result;
                }
            }

            return null;
        }

        public V GetCopyOfConfig(K configId)
        {
            var cfg = GetConfig(configId);
            var result = ObjectDeepCopy.Clone(cfg);
            if (_configIdFieldInfo != null && cfg != null)
            {
                _configIdFieldInfo.SetValue(result, configId);
            }

            return result;
        }

        public void ForEachConfig(Action<V> action)
        {
            lock (_sync)
            {
                if (_configCollection != null)
                {
                    foreach (var pair in _configCollection.Data)
                    {
                        action(pair.Value);
                    }
                }
            }
        }

        public V GetConfig(Predicate<V> condition)
        {
            lock (_sync)
            {
                foreach (var pair in _configCollection.Data)
                {
                    if (condition(pair.Value))
                    {
                        return pair.Value;
                    }
                }
            }

            return null;
        }

        public void Clear()
        {
            lock (_sync)
            {
                _configCollection.Data.Clear();
            }
        }

        public void Replace(K key, V value)
        {
            lock (_sync)
            {
                _configCollection.Data[key] = value;
            }
        }

        public string Dump()
        {
            lock (_sync)
            {
                var result = String.Empty;
                try
                {
                    result = JsonConvert.SerializeObject(_configCollection, Formatting.Indented, SerializerSettings);
                }
                catch (Exception e)
                {
                    Debug.LogError($"collection config storage {GetType()} :: error during dump configs \n exception {e}");
                }

                return result;
            }
        }
    }
    
    
}