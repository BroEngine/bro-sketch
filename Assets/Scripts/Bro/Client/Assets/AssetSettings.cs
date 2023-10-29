using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;

namespace Bro.Client
{
    public class AssetSettings<T> : ScriptableObject
    #if UNITY_EDITOR
        , IPreprocessBuildWithReport
    #endif    
    where T : ScriptableObject
    {
        #if UNITY_EDITOR
        int IOrderedCallback.callbackOrder => 0;
        void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
        {
            BuildValidate();
        }
        #endif

        protected virtual void BuildValidate()
        {
            
        }

        public static string Name
        {
            get
            {
                if (Attribute.IsDefined(typeof(T), typeof(AssetSettingsAttribute)))
                {
                    var attributeValue = Attribute.GetCustomAttribute(typeof(T), typeof(AssetSettingsAttribute)) as AssetSettingsAttribute;
                    if (!string.IsNullOrEmpty(attributeValue.Name))
                    {
                        return attributeValue.Name;
                    }
                }

                return typeof(T).Name;
            }
        }

        public static string SavingPath
        {
            get
            {
                if (Attribute.IsDefined(typeof(T), typeof(AssetSettingsAttribute)))
                {
                    var attributeValue = Attribute.GetCustomAttribute(typeof(T), typeof(AssetSettingsAttribute)) as AssetSettingsAttribute;
                    if (!string.IsNullOrEmpty(attributeValue.Path))
                    {
                        return Path.Combine("Assets", attributeValue.Path);
                    }
                }

                return "Assets/Resources";
            }
        }

        public static string LoadingPath
        {
            get
            {
                var savingPath = SavingPath;
                if (!savingPath.Contains("Resources"))
                {
                    Debug.LogError("asset setting :: settings class " + Name + " not contain resource folder in path");
                    return string.Empty;
                }

                var splits = savingPath.Split(new string[] {"Resources"}, StringSplitOptions.None);
                return splits[splits.Length - 1].TrimStart('/');
            }
        }

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    LoadInstanceForResources();

                    #if UNITY_EDITOR
                    if (_instance == null)
                    {
                        Debug.Log("asset settings :: resource file not found, type = " + typeof(T) + ", creating asset");
                        CreateSettingsAsset();

                        LoadInstanceForResources();

                        if (_instance == null)
                        {
                            Debug.LogError("asset settings :: reload error, type = " + typeof(T) + ", looks like creation failed");
                        }
                    }
                    #endif
                }

                return _instance;
            }
            set { _instance = value; }
        }

        private static void LoadInstanceForResources()
        {
            _instance = null;

            try
            {
                _instance = Resources.Load(ValidatePath(Path.Combine(LoadingPath, Name))) as T;
            }
            catch (Exception ex)
            {
                Debug.LogError($"asset settings :: type = {typeof(T)}, loading error = {ex.Message}");
            }
        }

        private static string ValidatePath(string path)
        {
            #if UNITY_STANDALONE_WIN
            return path.Replace("\\","/");
            #endif
            return path;
        }

        #if UNITY_EDITOR
        private static void CreateSettingsAsset()
        {
            _instance = CreateInstance<T>();

            var savingPath = SavingPath;
            if (!Directory.Exists(savingPath))
            {
                Directory.CreateDirectory(savingPath);
            }

            var fullPath = ValidatePath(Path.Combine(savingPath, $"{Name}.asset"));
            UnityEditor.AssetDatabase.CreateAsset(_instance, fullPath);
        }

        protected static void DirtyEditor()
        {
            UnityEditor.EditorUtility.SetDirty(Instance);
        }
        #endif
    }
}