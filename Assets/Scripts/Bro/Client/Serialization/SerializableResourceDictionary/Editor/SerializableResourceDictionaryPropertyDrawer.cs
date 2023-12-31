#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
// using UnityEngine.AddressableAssets;

namespace Bro.Client
{
    [CustomPropertyDrawer(typeof(SerializableResourceDictionary<,>), true)]
    public class SerializableResourceDictionaryPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                var keysProp = property.FindPropertyRelative("_keys");
                return (keysProp.arraySize + 2) * (EditorGUIUtility.singleLineHeight + 1f);
            }
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var expanded = property.isExpanded;
            var r = GetNextRect(ref position);
            property.isExpanded = EditorGUI.Foldout(r, property.isExpanded, label);

            if (expanded)
            {
                var level = EditorGUI.indentLevel;
                EditorGUI.indentLevel = level + 1;

                var keysProperty = property.FindPropertyRelative("_keys");
                var valuesProperty = property.FindPropertyRelative("_values");
                var enumProperty = property.FindPropertyRelative("_enum");
                
                var count = keysProperty.arraySize;
                if (valuesProperty.arraySize != count)
                {
                    valuesProperty.arraySize = count;
                }

                for (var i = 0; i < count; i++)
                {
                    r = GetNextRect(ref position);
                    var buttonWidth = r.height * 1.5f;
                    var keyWidth = r.width * 0.3f;
                    var valueWidth = Mathf.Abs(r.width - keyWidth - buttonWidth);
                    var keyRect = new Rect(r.xMin, r.yMin, keyWidth, r.height);
                    var valueRect = new Rect(keyRect.xMax, r.yMin, valueWidth, r.height);
                    var buttonRect = new Rect(valueRect.xMax, r.yMin, buttonWidth, r.height);

                    var keyProperty = keysProperty.GetArrayElementAtIndex(i);
                    var valueProperty = valuesProperty.GetArrayElementAtIndex(i);

                    DrawKey(keyRect, keyProperty, enumProperty);
                    DrawValue(valueRect, valueProperty);

                    if (GUI.Button(buttonRect, "-"))
                    {
                        keysProperty.DeleteArrayElementAtIndex(i);
                        valuesProperty.DeleteArrayElementAtIndex(i);
                        Undo.RecordObject(property.serializedObject.targetObject, $"SerializableResourceDictionary_{property.name}");
                        return;
                    }
                }

                EditorGUI.indentLevel = level;

                r = GetNextRect(ref position);
                var addWidth = EditorGUIUtility.singleLineHeight * 1.5f;
                var orderWidth = EditorGUIUtility.singleLineHeight * 4f;
                var addRect = new Rect(r.xMax - addWidth, r.yMin, addWidth, EditorGUIUtility.singleLineHeight);
                var orderRect = new Rect(r.xMax - addWidth - orderWidth, r.yMin, orderWidth, EditorGUIUtility.singleLineHeight);

                if (GUI.Button(addRect, "+"))
                {
                    // ((SerializableResource) (GetParent(keysProperty))).A();
                    ((SerializableResource) (GetParent(keysProperty))).Add();
                    // AddKeyElement(keysProperty);
                    // valuesProperty.arraySize = keysProperty.arraySize;
                    // Undo.RecordObject(property.serializedObject.targetObject, $"SerializableResourceDictionary_{property.name}");
                }
                
                r = GetNextRect(ref position);
                DrawUnusedKeys(r, keysProperty);
            }
        }

        public void DrawUnusedKeys(Rect area, SerializedProperty properties)
        {
            if (!properties.arrayElementType.Equals(typeof(Enum).Name))
            {
                return;
            }

            var names = new List<string>(properties.enumNames);
            for (int i = properties.arraySize - 1; i >= 0; i--)
            {
                var enumValueIndex = properties.GetArrayElementAtIndex(i).enumValueIndex;
                names.RemoveAt(enumValueIndex);
            }

            if (names.Count > 0)
            {
                var originalColor = GUI.contentColor;
                GUI.contentColor = Color.yellow;
                EditorGUI.LabelField(area, "Unused enum values:");
                for (int i = 0; i < names.Count; i++)
                {
                    var newRect = new Rect(area.xMin, area.yMax + area.height * i, area.width, area.height);
                    EditorGUI.LabelField(newRect, names[i]);
                }
                GUI.contentColor = originalColor;
            }
        }

        protected virtual void DrawKey(Rect area, SerializedProperty keyProperty, SerializedProperty enumProperty)
        {
            var v = keyProperty.stringValue;
            
            if ( ! ((SerializableResource) (GetParent(keyProperty))).IsValidKey(v) )
            {
                GUI.backgroundColor = Color.red;
            }

            // 123
            // var names = new List<string>(enumProperty.enum);
            // Debug.LogError("! " + names[0]);
            // var v = keyProperty.stringValue;
            //
            // if (!names.Contains(v))
            // {
            //     GUI.backgroundColor = Color.red;
            // }

            EditorGUI.PropertyField(area, keyProperty, GUIContent.none, false);
            
            GUI.backgroundColor = Color.white;
        }

        protected virtual void DrawValue(Rect area, SerializedProperty valueProperty)
        {
            var isEmpty = IsValueEmpty(valueProperty);
            if (isEmpty)
            {
                GUI.backgroundColor = Color.red;
            }

            EditorGUIUtility.labelWidth = 0.1f;
            EditorGUI.PropertyField(area, valueProperty, GUIContent.none, false);

            if (isEmpty)
            {
                GUI.backgroundColor = Color.white;
            }
        }

        public bool IsValueEmpty(SerializedProperty valueProperty)
        {
            if ((valueProperty.propertyType.Equals(SerializedPropertyType.ObjectReference) && valueProperty.objectReferenceValue.IsNull()) ||
                (valueProperty.propertyType.Equals(SerializedPropertyType.String) &&  string.IsNullOrEmpty(valueProperty.stringValue)))
            {
                return true;
            }
            else if (valueProperty.propertyType.Equals(SerializedPropertyType.Generic))
            {
                // if (valueProperty.type.Equals(typeof(AssetReference).Name))
                // {
                //     var guid = valueProperty.FindPropertyRelative("m_AssetGUID");
                //     return string.IsNullOrEmpty(guid.stringValue);
                // }
            }

            return false;
        }

        private static Rect GetNextRect(ref Rect position)
        {
            var r = new Rect(position.xMin, position.yMin, position.width, EditorGUIUtility.singleLineHeight);
            var h = EditorGUIUtility.singleLineHeight + 1f;
            position = new Rect(position.xMin, position.yMin + h, position.width, position.height = h);
            return r;
        }

        private static void AddKeyElement(SerializedProperty keysProperties)
        {
            keysProperties.arraySize++;
            var keyProperty = keysProperties.GetArrayElementAtIndex(keysProperties.arraySize - 1);

            switch (keyProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                    {
                        var value = 0;
                        for (var i = 0; i < keysProperties.arraySize - 1; i++)
                        {
                            if (keysProperties.GetArrayElementAtIndex(i).intValue == value)
                            {
                                value++;
                                i = -1;
                            }
                        }
                        keyProperty.intValue = value;
                    }
                    break;

                case SerializedPropertyType.String:
                    {
                        keyProperty.stringValue = string.Empty;
                    }
                    break;

                case SerializedPropertyType.Enum:
                    {
                        var value = 0;
                        if (keysProperties.arraySize > 1)
                        {
                            var first = keysProperties.GetArrayElementAtIndex(0);
                            for (var i = 0; i < keysProperties.arraySize - 1; i++)
                            {
                                if (keysProperties.GetArrayElementAtIndex(i).enumValueIndex == value)
                                {
                                    value++;
                                    i = -1;
                                }
                            }
                        }
                        keyProperty.enumValueIndex = value;
                    }
                    break;

                default:
                    throw new InvalidOperationException("can not handle type == " + keyProperty.propertyType + " as key");
            }
        }
        
        
        
        
        
        
        public object GetParent(SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach(var element in elements.Take(elements.Length-1))
            {
                if(element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[","").Replace("]",""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }
            return obj;
        }
 
        public object GetValue(object source, string name)
        {
            if(source == null)
                return null;
            var type = source.GetType();
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if(f == null)
            {
                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if(p == null)
                    return null;
                return p.GetValue(source, null);
            }
            return f.GetValue(source);
        }
 
        public object GetValue(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as IEnumerable;
            var enm = enumerable.GetEnumerator();
            while(index-- >= 0)
                enm.MoveNext();
            return enm.Current;
        }
    }
}

#endif