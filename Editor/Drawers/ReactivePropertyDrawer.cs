using UnityEditor;
using UnityEngine;

namespace MVVM.Editor
{
    [CustomPropertyDrawer(typeof(ReactiveProperty<>))]
    public class ReactivePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var valueProperty = property.FindPropertyRelative("_value");
            EditorGUI.PropertyField(position, valueProperty, label, valueProperty.hasVisibleChildren);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.hasChildren)
                return base.GetPropertyHeight(property, label);
            property.isExpanded = true;
            return EditorGUI.GetPropertyHeight(property, label, true) -
                   EditorGUI.GetPropertyHeight(property, label, false);
        }
    }
}