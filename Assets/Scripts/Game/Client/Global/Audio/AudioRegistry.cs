using System.Linq;
using Bro.Client;
using UnityEngine;

namespace Game.Client
{
    [AssetSettings("registry_audio", "Resources/Registries")]
    public class AudioRegistry : AssetSettings<AudioRegistry>
    {
         #if UNITY_EDITOR
        [UnityEditor.MenuItem("Registries/Audio")]
        public static void Edit()
        {
            Instance = null;
            UnityEditor.Selection.activeObject = Instance;
            DirtyEditor();
        }
        #endif
        
        [SerializeField] private SerializableResourceDictionary<AudioType, AudioClip> _data = new SerializableResourceDictionary<AudioType, AudioClip>();
        [SerializeField] private SerializableResourceDictionary<AudioType, float> _volume = new SerializableResourceDictionary<AudioType, float>();
        
        public AudioClip GetClip(AudioType audioType)
        {
            if (!_data.ContainsKey(audioType.GetDescription()))
            {
                Debug.LogError($"no audio with type = {audioType.GetDescription()} {audioType}");
                return _data.Dict.FirstOrDefault().Value;
            }
            return _data[audioType.GetDescription()];
        }
        
        public float GetVolume(AudioType audioType)
        {
            if (_volume.ContainsKey(audioType.GetDescription()))
            {
                return _volume[audioType.GetDescription()];
            }
            return 1.0f;
        }
    }
}