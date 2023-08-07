using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Compilation;
using UnityEngine;

namespace MVVM.Editor
{
    [InitializeOnLoad]
    public static class BindersGenerator
    {
        private const string PATH_TO_GENERATED_FILE = "Assets/Scripts/Generated/Resolvers/ResolversGenerated.cs";
        private const string GENERATED_FILE_NAME = "ResolversGenerated.cs";
        private const string ALL_REACTIVE_FIELDS_COUNT_KEY = "AllReactiveFields";

        static BindersGenerator()
        {
            CompilationPipeline.assemblyCompilationFinished += OnCompilationFinished;
        }

        private static void OnCompilationFinished(string s, CompilerMessage[] compilerMessages)
        {
            //if has compile errors try remove generated file
            if (compilerMessages.Count(m => m.type == CompilerMessageType.Error) > 0)
            {
                bool errorInGeneratedClass = compilerMessages.Any(x => Path.GetFileName(x.file) == GENERATED_FILE_NAME);

                if (errorInGeneratedClass)
                {
                    RemoveFile();
                    EditorUtility.RequestScriptReload();
                    //CompilationPipeline.RequestScriptCompilation();
                }
            }
        }

        private static void RemoveFile()
        {
            EditorApplication.LockReloadAssemblies();
            string path = $"{Application.dataPath}/Scripts/Generated/Resolvers/ResolversGenerated.cs";
            if (File.Exists(path))
            {
                File.Delete(path);
                EditorPrefs.DeleteKey(ALL_REACTIVE_FIELDS_COUNT_KEY);
            }

            EditorApplication.UnlockReloadAssemblies();
        }


        [DidReloadScripts]
        private static void Generate()
        {
            string filePath = $"{Application.dataPath}/Scripts/Generated/Resolvers/ResolversGenerated.cs";
            if (!File.Exists(filePath))
                EditorPrefs.DeleteKey(ALL_REACTIVE_FIELDS_COUNT_KEY); //clear cache when not exists
            
            var types = TypeCache
                .GetTypesDerivedFrom(typeof(MonoBehaviour))
                .Where(p =>
                    (p.IsPublic || p.IsNestedPublic)
                    && !p.IsAbstract)
                .OrderBy(x=> x.FullName);


            var count = types.Sum(x => x.GetAllReactive<IReactiveProperty>().Count);
            int lastCount = EditorPrefs.GetInt(ALL_REACTIVE_FIELDS_COUNT_KEY);
            if (count == lastCount)
                return;


            string content =
                @"using System; using System.Collections.Generic;using MVVM;using UnityEngine; namespace MVVM.Generated{";
            List<Type> reactiveTypes = new();
            foreach (var type in types)
            {
                var allReactive = type
                    .GetAllReactive<IReactiveProperty>()
                    .Select(name => $@"{{ ""{name}"", o => o.{name} }},")
                    .ToList();

                if (allReactive.Count == 0)
                    continue;

                reactiveTypes.Add(type);
                content +=
                    $@"    
                public class Resolver_{type.FullName.Replace('.', '_')}: IResolver
                {{
                        private Dictionary<string, Func<{type.FullName}, IReactiveProperty>> map = new()
                        {{
                            {string.Join("\r\n", allReactive)}
                        }};

                        public IReactiveProperty Map(UnityEngine.Object target, string name)
                        {{
                            return map[name].Invoke(target as {type.FullName});
                        }}
                }}
                ";
            }


            if (reactiveTypes.Count == 0)
            {
                RemoveFile();
                return;
            }

            content += $@"
        public static class BindersLoader
        {{
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
            private static void InitResolvers()
            {{
                Binders.AddResolvers(new()
                {{
                    {string.Join("\r\n", reactiveTypes.Select(t => $"{{typeof({t.FullName}),new Resolver_{t.FullName.Replace('.', '_')}()}},"))}    
                }});
            }}
        }}}}";

            string path = $"{UnityEngine.Application.dataPath}/Scripts/Generated/Resolvers";
            Directory.CreateDirectory(path);
            File.WriteAllText($"{path}/ResolversGenerated.cs", content);
            EditorPrefs.SetInt(ALL_REACTIVE_FIELDS_COUNT_KEY, count);
        }
    }
}