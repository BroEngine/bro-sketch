using System.Collections.Generic;
using UnityEngine;

namespace Bro.Client.UI
{
    [AssetSettings("registry_ui_elements", "Resources/Registries")]
    public class UIElementsRegistry : AssetSettings<UIElementsRegistry>
    {
        [SerializeField] private List<Window> _global = new List<Window>();
        [SerializeField] private List<Window> _lobby = new List<Window>();
        [SerializeField] private List<Window> _battle = new List<Window>();

        
        #if UNITY_EDITOR
        [UnityEditor.MenuItem("Registries/UI Elements")]
        public static void Edit()
        {
            Instance = null;
            UnityEditor.Selection.activeObject = Instance;
            DirtyEditor();
        }
        #endif
        
        public Window Get<T>() where T : Window
        {
            var window = Get<T>(_global, _lobby, _battle);
            return window;
        }
        
        private Window Get<T>(params List<Window>[] lists) where T : Window
        {
            var targetType = typeof(T);
            foreach (var list in lists)
            {
                foreach (var window in list)
                {
                    if (window != null)
                    {
                        if (window.GetType() == targetType)
                        {
                            return window;
                        }
                    }
                }
            }
            return null;
        }
    }
}