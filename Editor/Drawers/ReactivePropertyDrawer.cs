using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MVVM.Editor
{
    [CustomPropertyDrawer(typeof(ReactiveProperty<>))]
    public class ReactivePropertyDrawer : PropertyDrawer
    {
        private PropertyInfo _valueProp;
        private PropertyInfo ValueProp => _valueProp??= typeof(ReactiveProperty<>).GetProperty("Value");
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label,property);
            var valueProperty = property.FindPropertyRelative("_value");
            position = EditorGUI.PrefixLabel(position, label);
            EditorGUI.PropertyField(position, valueProperty, GUIContent.none);
            EditorGUI.EndProperty();
        }
    }
}