using System;
using Bro.Client.Encryption;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Bro.Client.Storage
{
    public class LocalProfileStorage<T> where T : new()
    {
        private readonly string _storageKey;
        private T _model = new T();
        private string _encryptionKey;
        private bool _isEncryption;
        
        public T Model => _model;
        public string Json => JsonConvert.SerializeObject(_model);
        
        public LocalProfileStorage(string storageKey, string encryptionKey)
        {
            _storageKey = storageKey;
            _encryptionKey = encryptionKey;
            _isEncryption = !string.IsNullOrEmpty(encryptionKey);
        }

        public void Reset()
        {
            _model = new T();
            Save(true);
        }

        public async UniTask LoadAsync()
        {
            var json = PlayerPrefs.GetString(_storageKey);
            
            if (!string.IsNullOrEmpty(json))
            {
                if (!_isEncryption || json.Contains("{")) /* try parse not encrypted data */
                {
                    try
                    {
                        var o =  await UniTask.RunOnThreadPool(() => JsonConvert.DeserializeObject<T>(json));
                        if (o != null)
                        {
                            _model = o;
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"local profile storage :: cant load not encrypted save {e.Message}");
                    }
                }
                
                try
                {
                    var encryptor = new AES(_encryptionKey);
                    json =  await UniTask.RunOnThreadPool(() =>encryptor.Decrypt(json));
                    
                    var o = await UniTask.RunOnThreadPool(() =>JsonConvert.DeserializeObject<T>(json));
                    if (o != null)
                    {
                        _model = o;
                        return;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"local profile storage :: cant load encrypted save {e.Message}");
                }
            }
        }
        
        public void Save(bool force)
        {
            SaveAsync(force).Forget();
        }

        private async UniTask SaveAsync(bool force)
        {
            var json = await UniTask.RunOnThreadPool(() =>  JsonConvert.SerializeObject(_model));

            if (_isEncryption)
            {
                try
                {
                    var encryptor = new AES(_encryptionKey);
                    var data = await UniTask.RunOnThreadPool(() => encryptor.Encrypt(json));
                    json = data;
                }
                catch (Exception e)
                {
                    Debug.LogError($"local profile storage :: failed to encrypt save {e.Message}");
                }
            }

            PlayerPrefs.SetString(_storageKey, json);
            if (force)
            {
                PlayerPrefs.Save();
            }
        }
    }
}