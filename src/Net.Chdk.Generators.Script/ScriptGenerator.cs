using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Generators.Script
{
    sealed class ScriptGenerator : IScriptGenerator
    {
        public void GenerateScript(string targetPath, string name, IDictionary<string, string> substitutes)
        {
            var path1 = Path.Combine(Directories.Data, Directories.Script, $"{name}.txt");
            var text = File.ReadAllText(path1);
            foreach (var kvp in substitutes)
            {
                var str1 = $"{{{kvp.Key}}}";
                var str2 = kvp.Value;
                text = text.Replace(str1, str2);
            }
            var path2 = Path.Combine(targetPath, "extend.m");
            File.WriteAllText(path2, text);
        }
    }
}
