using System;
using UnityEngine;

namespace Game.Client
{
    [Serializable]
    public struct VehicleRegistryItem
    {
        [SerializeField] private GameObject _prefab;

        public GameObject Prefab => _prefab;
    }
}