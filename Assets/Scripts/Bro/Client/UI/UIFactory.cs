using System;
using Bro.Client.Context;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Bro.Client.UI
{
    public class UIFactory
    {
        private readonly IClientContext _context;
        private readonly Transform _root;
        
        public UIFactory(IClientContext context, Transform root)
        {
            _context = context;
            _root = root;
        }
        
        public T Create<T>() where T : Window
        {
            var attribute = GetAttribute<T>();
            var prefab = UIElementsRegistry.Instance.Get<T>();
            if (prefab != null)
            {
                var window = Object.Instantiate(prefab, _root).GetComponent<T>();
                window.gameObject.SetActive(false);
                window.Setup(_context, attribute.ItemType);
                return window;
            }

            Debug.LogError($"no prefab found for window = ${typeof(T)}");
            return null;
        }
        
        /// <summary>
        /// Getting window prefab name from attribute
        /// </summary>
        private WindowAttribute GetAttribute<T>() where T : Window
        {
            if (Attribute.IsDefined(typeof(T), typeof(WindowAttribute)))
            {
                return Attribute.GetCustomAttribute(typeof(T), typeof(WindowAttribute)) as WindowAttribute;
            }

            Debug.LogError($"ui component :: window {typeof(T)} has no window attribute");
            return null;
        }
    }
}