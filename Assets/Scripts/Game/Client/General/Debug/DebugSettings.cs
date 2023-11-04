using Bro.Client;
using UnityEngine;

namespace Game.Client
{
    [AssetSettings("settings_debug", "Resources/Settings")]
    public class DebugSettings : AssetSettings<DebugSettings>
    {
        #if UNITY_EDITOR
        [UnityEditor.MenuItem("Settings/Debug Settings")]
        public static void Edit()
        {
            Instance = null;
            UnityEditor.Selection.activeObject = Instance;
            DirtyEditor();
        }
        #endif

        [Space]
        [SerializeField] private bool _isLoadCustomLevel;
        [SerializeField] private string _customLevelId;
        
        public bool IsLoadCustomLevel => _isLoadCustomLevel;
        public string CustomLevelId => _customLevelId;
    }
}