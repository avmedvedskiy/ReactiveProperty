using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MVVM;
using UnityEditor;
using UnityEngine;

public static class BindersGenerator
{
    [MenuItem("Assets/Fix Compile Binding Errors")]
    private static void ManualGeneration()
    {
        Generate();
        AssetDatabase.Refresh();
    }
    
    //[UnityEditor.Callbacks.DidReloadScripts]
    private static void Generate()
    {
        var types = TypeCache
            .GetTypesDerivedFrom(typeof(MonoBehaviour))
            .Where(p =>
                (p.IsPublic || p.IsNestedPublic) &&
                !p.IsAbstract &&
                !p.IsGenericType);

        string content = @"using System; using System.Collections.Generic;using MVVM;using MVVM.Test;using UnityEngine;";
        List<Type> reactiveTypes = new();
        foreach (var type in types)
        {
            //Debug.Log($"Class name = {type.Name}");
            var fields = type
                .GetFields()
                .Where(f => f.FieldType.GetInterfaces().Contains(typeof(IReactiveProperty)))
                .Select(fi => $@"{{ ""{fi.Name}"", o => o.{fi.Name} }},")
                .ToList();
            if(fields.Count == 0)
                continue;
            
            reactiveTypes.Add(type);
            content += 
                $@"    
                public class Resolver_{type.Name}: IResolver
                {{
                        private Dictionary<string, Func<{type.Name}, IReactiveProperty>> map = new()
                        {{
                            {string.Join("\r\n", fields)}
                        }};

                        public IReactiveProperty Map(UnityEngine.Object target, string name)
                        {{
                            return map[name].Invoke(target as {type.Name});
                        }}
                }}
                " ;
        }
        
        content += $@"
        namespace MVVM{{
        public static class BindersLoader
        {{
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
            private static void InitResolvers()
            {{
                Binders.AddResolvers(new()
                {{
                    {string.Join("\r\n", reactiveTypes.Select(t => $"{{typeof({t.Name}),new Resolver_{t.Name}()}},"))}    
                }});
            }}
        }}}}";

        string path = $"{UnityEngine.Application.dataPath}/Scripts/Generated/Resolvers";
        Directory.CreateDirectory(path);
        File.WriteAllText($"{path}/ResolversGenerated.cs", content);
    }
}