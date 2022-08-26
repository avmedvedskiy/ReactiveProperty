using UnityEditor;
using UnityEngine;

namespace MVVM.Editor
{
    [CustomPropertyDrawer(typeof(ReactiveEvent))]
    public class ReactiveEventDrawer : PropertyDrawer
    {
        private const string NOTIFY_METHOD_NAME = "Notify";
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var rectButton = position;
            rectButton.width -= EditorGUIUtility.labelWidth;
            rectButton.x += EditorGUIUtility.labelWidth;
            if (GUI.Button(rectButton,NOTIFY_METHOD_NAME ))
            {
                property.InvokeMethod(NOTIFY_METHOD_NAME);
            }
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndProperty();
        }
    }
}