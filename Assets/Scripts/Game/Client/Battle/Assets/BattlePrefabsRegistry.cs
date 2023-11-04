using Bro.Client;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Client.Battle
{
    [AssetSettings("registry_battle_prefabs", "Resources/Registries")]
    public class BattlePrefabsRegistry : AssetSettings<BattlePrefabsRegistry>
    {
        #if UNITY_EDITOR
        [UnityEditor.MenuItem("Registries/Battle Prefabs")]
        public static void Edit()
        {
            Instance = null;
            UnityEditor.Selection.activeObject = Instance;
            DirtyEditor();
        }
        #endif

        // [SerializeField] private GameObject _prefab;
        
        // public GameObject Prefab => _prefab;
    }
}