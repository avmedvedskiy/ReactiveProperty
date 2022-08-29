using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MVVM;
using UnityEditor;
using UnityEngine;

namespace MVVM.Editor
{
    public static class BindersGenerator
    {
        [MenuItem("Assets/Fix Compile Binding Errors")]
        private static void ManualGeneration()
        {
            Remove();
            Generate();
            AssetDatabase.Refresh();
        }

        private static void Remove()
        {
            string path = $"{UnityEngine.Application.dataPath}/Scripts/Generated/Resolvers/ResolversGenerated.cs";
            if (File.Exists(path))
                File.Delete(path);
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void Generate()
        {
            var types = TypeCache
                .GetTypesDerivedFrom(typeof(MonoBehaviour))
                .Where(p =>
                    (p.IsPublic || p.IsNestedPublic)
                    && !p.IsAbstract);

            string content =
                @"using System; using System.Collections.Generic;using MVVM;using UnityEngine; namespace MVVM.Generated{";
            List<Type> reactiveTypes = new();
            foreach (var type in types)
            {
                var allReactive = type
                    .GetAllReactive()
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
                Remove();
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
        }
    }
}