using System;
using System.Collections.Generic;
using Bro.Client.Context;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bro.Client.Configs
{
    public class ConfigModule : IClientContextModule
    {
        private readonly List<IConfigStorage> _configStorages = new List<IConfigStorage>();

        void IClientContextElement.Setup(IClientContext context)
        {
        }

        async UniTask IClientContextModule.Load()
        {
            await UniTask.Yield();
        }

        async UniTask IClientContextModule.Unload()
        {
            await UniTask.Yield();
        }

        public T GetConfigStorage<T>() where T : IConfigStorage
        {
            foreach (var storage in _configStorages)
            {
                if (storage is T result)
                {
                    return result;
                }
            }

            return default(T);
        }

        public IConfigStorage GetConfigStorage(Predicate<IConfigStorage> isSuitable)
        {
            foreach (var storage in _configStorages)
            {
                if (isSuitable(storage))
                {
                    return storage;
                }
            }

            return null;
        }

        public async UniTask LoadConfigsAsync(List<ConfigStorageDescription> storagesDescriptionToLoad)
        {
            var loadStorageTasks = new List<UniTask>();
            foreach (var description in storagesDescriptionToLoad)
            {
                loadStorageTasks.Add(LoadConfigAsync(description));
            }

            await UniTask.WhenAll(loadStorageTasks);
        }

        public async UniTask LoadConfigAsync(ConfigStorageDescription storageDescriptionToLoad)
        {
            await ProcessLoadLocalConfig(storageDescriptionToLoad);
        }

        private async UniTask ProcessLoadLocalConfig(ConfigStorageDescription description)
        {
            var resourceRequest = UnityEngine.Resources.LoadAsync(description.LocalStoragePath);
            await resourceRequest;
            var textAsset = (UnityEngine.TextAsset)resourceRequest.asset;
            var storage = description.Creator.Invoke();
            
            Debug.Assert(textAsset!= null, $"failed to load file = {description.LocalStoragePath}");
            
            await storage.InitializeAsync(textAsset.text, 0, description.Identifier);
            _configStorages.Add(storage);
            // var persistentData = PersistentStorage.LoadText(preferences.PersistentStoragePath);
        }
    }
}