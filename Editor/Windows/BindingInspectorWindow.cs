using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVVM.Editor
{
    public sealed class BindingInspectorWindow : EditorWindow, IHasCustomMenu
    {
        #region Init

        [MenuItem("Tools/Reactive/BindingInspectorWindow")]
        private static void Window()
        {
            GetWindow<BindingInspectorWindow>()
                .titleContent = new GUIContent("BindingInspectorWindow<IView>", "!");
        }

        #endregion

        private readonly GUIContent _addNewViewComponent = new("Add View", "Add New View to selected GameObject");
        private readonly GUIContent _setTargetToAllViews = new("Set All", "Set Root GameObject To All Views");
        private readonly GUIContent _deleteView = new("Delete View");

        private IEnumerable<UnityEditor.Editor> _targetEditors;

        private Vector2 _scrollPos;
        private GameObject _activeGameObject;
        private GameObject _rootGameObject;

        [NonSerialized] private bool _locked = true;
        [NonSerialized] private GUIStyle _lockButtonStyle;

        private List<Type> _cachedReactiveViewTypes;

        private List<Type> CachedReactiveViewTypes =>
            _cachedReactiveViewTypes ??= TypeCache
                .GetTypesDerivedFrom(typeof(View<>))
                .ToList();

        public void OnEnable()
        {
            SelectionChanged();
            Selection.selectionChanged += SelectionChanged;
        }

        public void OnDisable()
        {
            Selection.selectionChanged -= SelectionChanged;
        }

        private void OnGUI()
        {
            EditorGUILayout.ObjectField(_rootGameObject, typeof(Object), true);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(_setTargetToAllViews))
            {
                SetTargetToAllViews();
            }

            GUI.backgroundColor = Color.green;
            if (EditorGUILayout.DropdownButton(_addNewViewComponent, FocusType.Passive))
            {
                ShowAddViewItemDropdown();
            }

            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            DrawEditors();
        }

        private void DrawEditors()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            EditorGUILayout.Space(10f);
            if (_activeGameObject)
            {
                foreach (var editor in _targetEditors)
                {
                    if (editor == null)
                        continue;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(editor.target, typeof(Object), true);
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button(_deleteView))
                    {
                        DestroyImmediate(editor.target);
                    }

                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.EndHorizontal();

                    if (editor.target != null)
                        editor.OnInspectorGUI();
                    EditorGUILayout.Space(20f);
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void ShowAddViewItemDropdown()
        {
            //remove IReactiveView
            GenericMenu nodesMenu = new GenericMenu();
            foreach (var viewType in CachedReactiveViewTypes)
            {
                var genericType = viewType.GenericTypeArgumentDeep();
                string group = genericType != null ? $"{genericType.Name}/" : string.Empty;
                nodesMenu.AddItem(new GUIContent($"{group}{viewType.Name}"), false,
                    () => AddNewViewComponent(viewType));
            }

            nodesMenu.ShowAsContext();
        }

        private void AddNewViewComponent(Type viewType)
        {
            var component = _activeGameObject.AddComponent(viewType);
            SetTargetToView(new SerializedObject(component));
            SelectionChanged();
        }

        private void SetTargetToAllViews()
        {
            foreach (var editor in _targetEditors)
            {
                var obj = editor.serializedObject;
                SetTargetToView(obj);
            }
        }

        private void SetTargetToView(SerializedObject serializedObject)
        {
            SerializedProperty prop = serializedObject.GetIterator();
            if (prop.NextVisible(true))
            {
                do
                {
                    var relative = prop.FindPropertyRelative("_propertyName");
                    if (relative != null)
                    {
                        var target = prop.FindPropertyRelative("_target");
                        if (string.IsNullOrEmpty(relative.stringValue) || target.objectReferenceValue == null)
                            target.objectReferenceValue = _rootGameObject;
                    }
                } while (prop.NextVisible(false));
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void SelectionChanged()
        {
            _activeGameObject = Selection.activeGameObject;
            if (_activeGameObject == null)
                return;

            //добавить хоткей и 3 точечки
            _rootGameObject = _locked ? _activeGameObject.transform.root.gameObject : _activeGameObject;
            _targetEditors = _rootGameObject
                .GetComponentsInChildren(typeof(MonoBehaviour))
                .Where(x => CachedReactiveViewTypes.Contains(x.GetType()))
                .Select(UnityEditor.Editor.CreateEditor);

            Repaint();
        }

        /// <summary>
        /// Magic method which Unity detects automatically.
        /// </summary>
        private void ShowButton(Rect position)
        {
            _lockButtonStyle ??= "IN LockButton";
            _locked = GUI.Toggle(position, _locked, GUIContent.none, _lockButtonStyle);
        }

        void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Lock"), this._locked, () => { _locked = !_locked; });
        }
    }
}