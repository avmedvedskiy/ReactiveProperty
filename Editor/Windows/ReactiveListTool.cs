using System.IO;
using UnityEngine;

public class ReactiveListTool
{
    public void CreateModelScript(string modelName, string rootPath)
    {
        var fullScriptPath = Application.dataPath.Replace("Assets", $"{rootPath}/{modelName}.cs");

        using (StreamWriter outfile = new StreamWriter(fullScriptPath))
        {
            outfile.WriteLine($"public class {modelName}");
            outfile.WriteLine("{");
            outfile.WriteLine($"    public {modelName}()");
            outfile.WriteLine("    {");
            outfile.WriteLine("");
            outfile.WriteLine("    }");
            outfile.WriteLine("}");
        }
    }

    public void CreateModelViewScript(string modelName, string rootPath)
    {
        var fullScriptPath = Application.dataPath.Replace("Assets", $"{rootPath}/{modelName}View.cs");

        using (StreamWriter outfile = new StreamWriter(fullScriptPath))
        {
            outfile.WriteLine("using MVVM.Collections;");
            outfile.WriteLine("");
            outfile.WriteLine($"public class {modelName}View : ModelView<{modelName}>");
            outfile.WriteLine("{");
            outfile.WriteLine($"    public {modelName} Model" + " { get; private set; }");
            outfile.WriteLine("");
            outfile.WriteLine($"    public override void SetModel({modelName} model)");
            outfile.WriteLine("    {");
            outfile.WriteLine($"        Model = model;");
            outfile.WriteLine("    }");
            outfile.WriteLine("}");
        }
    }

    public void CreateModelListViewScript(string modelName, string rootPath)
    {
        var fullScriptPath = Application.dataPath.Replace("Assets", $"{rootPath}/{modelName}ListView.cs");

        using (StreamWriter outfile = new StreamWriter(fullScriptPath))
        {
            outfile.WriteLine("using MVVM.Collections;");
            outfile.WriteLine("using System.Collections.Generic;");
            outfile.WriteLine("");
            outfile.WriteLine($"public class {modelName}ListView : BaseListView<{modelName}, {modelName}View>");
            outfile.WriteLine("{");
            outfile.WriteLine($"    public ReactiveList<{modelName}> List" + " {get; } = new();");
            outfile.WriteLine("");
            outfile.WriteLine($"    public void Init(List<{modelName}> list)");
            outfile.WriteLine("    {");
            outfile.WriteLine("        List.AddRange(list);");
            outfile.WriteLine("    }");
            outfile.WriteLine("}");
        }
    }
}