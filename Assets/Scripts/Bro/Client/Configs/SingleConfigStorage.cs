using Bro.Client.Json;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Bro.Client.Configs
{
    public class SingleConfigStorage<T> : IConfigProvider<T>, IConfigStorage where T : class
    {
        public class SingleModel<TModel>
        {
            [JsonProperty("data")] public TModel Data;
        }
        
        private SingleModel<T> _configModel = new SingleModel<T>();
        private readonly object _sync = new object();
        
        private JsonSerializerSettings SerializerSettings => DefaultJsonSerializerSettings.AutoSettings;
        public string Identifier { get; private set; }
        public int Version { get; private set; }
        
        public T GetConfig()
        {
            return _configModel?.Data;
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
                var deserializedObject = await UniTask.RunOnThreadPool(() => JsonConvert.DeserializeObject<SingleModel<T>>(configData, SerializerSettings));

                lock (_sync)
                {
                    Version = version;
                    Identifier = identifier;
                    _configModel = deserializedObject;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"single config storage {GetType()} :: error during parsing configs \n exception {e}\n json = {configData}");
            }
        }

        public void Replace(T value)
        {
            if (_configModel == null)
            {
                _configModel = new SingleModel<T>();
            }
            _configModel.Data = value;
        }
        
        public string Dump()
        {
            lock (_sync)
            {
                return JsonConvert.SerializeObject(_configModel, Formatting.Indented, SerializerSettings);
            }
        }

        public void Reset()
        {
            lock (_sync)
            {
                _configModel = null;
            }
        }
    }
}