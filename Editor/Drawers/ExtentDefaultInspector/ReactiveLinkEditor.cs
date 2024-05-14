using System.Collections.Generic;
using System.Linq;
using MVVM.Editor;
using UnityEditor;
using UnityEngine;

namespace MVVM
{
    internal class ReactiveLinkEditor
    {
        private class ReactiveLinkData
        {
            public string Property { get; }
            public IView View { get; }
            public SerializedObject SerializedObject { get; }
            public ReactiveLinkData(string property, IView view)
            {
                Property = property;
                View = view;
                if (view is MonoBehaviour mono)
                    SerializedObject = new SerializedObject(mono);
            }
        }

        private readonly List<ReactiveLinkData> _links;
        public bool Expanded { get; set; }

        public ReactiveLinkEditor(MonoBehaviour target)
        {
            var allReactiveProperties = target.GetType().GetAllReactive<IReactiveProperty>();
            if (allReactiveProperties is { Count: > 0 })
            {
                var allView = target.GetComponentsInChildren<IView>();
                _links = allReactiveProperties
                    .Select(x =>
                        new ReactiveLinkData(x, allView.FirstOrDefault(view => view.IsPropertyEquals(x))))
                    .ToList();
                Expanded = true;
            }
        }

        public void OnInspectorGUI()
        {
            if (_links is { Count: > 0 })
            {
                Expanded = EditorGUILayout.Foldout(Expanded, "Reactive Property", true);
                if (Expanded)
                {
                    foreach (var link in _links)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel(link.Property);
                        EditorGUILayout.ObjectField((Object)link.View, typeof(Object), true);
                        EditorGUILayout.EndHorizontal();

                        EditorGUI.indentLevel++;
                        if (link.SerializedObject != null)
                            DoDrawDefaultInspector(link.SerializedObject);
                        EditorGUI.indentLevel--;
                        EditorGUILayout.Separator();
                    }
                }
            }
        }
        
        

        void DoDrawDefaultInspector(SerializedObject viewObject)
        {
            EditorGUI.BeginChangeCheck();
            viewObject.UpdateIfRequiredOrScript();
            SerializedProperty iterator = viewObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                if (iterator.propertyPath is not ("_target" or "m_Script"))
                    EditorGUILayout.PropertyField(iterator, true);
            }
            if (EditorGUI.EndChangeCheck())
                viewObject.ApplyModifiedProperties();
        }
    }
}