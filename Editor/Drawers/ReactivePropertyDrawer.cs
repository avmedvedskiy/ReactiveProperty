using UnityEditor;
using UnityEngine;

namespace MVVM.Editor
{
    [CustomPropertyDrawer(typeof(ReactiveProperty<>))]
    public class ReactivePropertyDrawer : PropertyDrawer
    {
        private const string NOTIFY_METHOD_NAME = "Notify";
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var valueProperty = property.FindPropertyRelative("_value");
            var rectButton = position;
            rectButton.width = 45f;
            rectButton.x = EditorGUIUtility.labelWidth - 30f;
            rectButton.height = EditorGUIUtility.singleLineHeight;
            if (GUI.Button(rectButton,NOTIFY_METHOD_NAME ))
            {
                property.InvokeMethod(NOTIFY_METHOD_NAME);
            }

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