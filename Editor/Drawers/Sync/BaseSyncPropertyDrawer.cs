using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVVM.Editor
{
    public class BaseSyncPropertyDrawer<T> : PropertyDrawer where T : IReactiveProperty
    {
        static readonly GUIContent _noneLabel = new GUIContent("None");

        private string type;

        //UnityEventDrawer 
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var targetProperty = property.FindPropertyRelative("_target");
            var nameProperty = property.FindPropertyRelative("_propertyName");

            var afterLabelPosition = EditorGUI.PrefixLabel(position, label);
            afterLabelPosition.width /= 3f;
            EditorGUI.ObjectField(afterLabelPosition, targetProperty, GUIContent.none);

            afterLabelPosition.x += afterLabelPosition.width;
            afterLabelPosition.width *= 2f;
            if (EditorGUI.DropdownButton(afterLabelPosition, DropdownContent(targetProperty, nameProperty),
                    FocusType.Keyboard))
            {
                //Debug.Log($"{property.displayName} DropdownButton");
                AddDropdown(targetProperty, nameProperty, fieldInfo);
            }

            ClearNameProperty(targetProperty, nameProperty);

            EditorGUI.EndProperty();
        }

        private static void ClearNameProperty(SerializedProperty targetProperty, SerializedProperty nameProperty)
        {
            if (targetProperty.objectReferenceValue is GameObject)
                nameProperty.stringValue = string.Empty;
        }

        private GUIContent DropdownContent(SerializedProperty targetProperty, SerializedProperty nameProperty)
        {
            if (targetProperty.objectReferenceValue == null || targetProperty.objectReferenceValue is GameObject ||
                string.IsNullOrEmpty(nameProperty.stringValue))
            {
                return _noneLabel;
            }

            return new GUIContent(GetName(targetProperty, nameProperty));
        }

        private static string GetName(SerializedProperty targetProperty, SerializedProperty nameProperty)
        {
            return $"{nameProperty.stringValue}";
        }

        private static string GetName(Component c, string propertyName)
        {
            return $"{c.GetType().Name}/{propertyName}";
        }

        private bool IsEquals(SerializedProperty targetProperty, Component component, SerializedProperty nameProperty,
            string propertyName)
        {
            return nameProperty.stringValue == propertyName &&
                   targetProperty.objectReferenceValue.GetType() == component.GetType();
        }

        private void AddDropdown(SerializedProperty targetProperty, SerializedProperty nameProperty,
            FieldInfo info)
        {
            var genericType = fieldInfo.FieldType.GenericTypeArguments.Length > 0
                ? fieldInfo.FieldType.GenericTypeArguments[0]
                : null;
            GenericMenu nodesMenu = new GenericMenu();
            var go = targetProperty.objectReferenceValue as GameObject ??
                     (targetProperty.objectReferenceValue as Component)?.gameObject;
            if (go != null)
            {
                foreach (var component in go.GetComponents<Component>())
                {
                    foreach (var propertyName in GetReactivePropertyNames(component.GetType(), genericType))
                    {
                        bool equalNames = IsEquals(targetProperty, component, nameProperty, propertyName);
                        nodesMenu.AddItem(new GUIContent(GetName(component, propertyName)), equalNames,
                            () => OnSelectTarget(component, propertyName, targetProperty, nameProperty));
                    }
                }
            }

            bool noneField = string.IsNullOrEmpty(nameProperty.stringValue);
            nodesMenu.AddItem(_noneLabel, noneField, () => OnSelectNone(targetProperty));
            nodesMenu.ShowAsContext();
        }

        private List<string> GetReactivePropertyNames(Type type, Type genericType)
        {
            return type.GetAllReactive<T>(genericType);
        }

        private void OnSelectTarget(Component component, string propName, SerializedProperty targetProperty,
            SerializedProperty nameProperty)
        {
            targetProperty.objectReferenceValue = component;
            nameProperty.stringValue = propName;
            nameProperty.serializedObject.ApplyModifiedProperties();
            targetProperty.serializedObject.ApplyModifiedProperties();
        }

        private void OnSelectNone(SerializedProperty targetProperty)
        {
            if (targetProperty.objectReferenceValue == null || targetProperty.objectReferenceValue is GameObject)
                return;

            targetProperty.objectReferenceValue = (targetProperty.objectReferenceValue as Component)?.gameObject;
            targetProperty.serializedObject.ApplyModifiedProperties();
        }
    }
}