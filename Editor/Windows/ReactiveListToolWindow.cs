using UnityEditor;
using UnityEngine;

public class ReactiveListToolWindow : EditorWindow
{
    private string _modelName;
    private Object _scriptsFolder;

    public ReactiveListTool ReactiveListTool { get; } = new();

    [MenuItem("Tools/Reactive/ReactiveList Tool")]
    private static void Init()
    {
        GetWindow<ReactiveListToolWindow>();
    }

    [MenuItem("GameObject/Reactive/ReactiveList Tool", false)]
    private static void Init(MenuCommand menuCommand)
    {
        Init();
    }

    private void OnGUI()
    {
        _modelName = EditorGUILayout.TextField("Model Name", _modelName);
        _scriptsFolder = EditorGUILayout.ObjectField("Scripts Folder", _scriptsFolder, typeof(DefaultAsset), false);

        if (!string.IsNullOrEmpty(_modelName) && _scriptsFolder != null && GUILayout.Button("Create"))
        {
            var path = AssetDatabase.GetAssetPath(_scriptsFolder);
            ReactiveListTool.CreateModelScript(_modelName, path);
            ReactiveListTool.CreateModelViewScript(_modelName, path);
            ReactiveListTool.CreateModelListViewScript(_modelName, path);
            AssetDatabase.Refresh();
        }
    }
}