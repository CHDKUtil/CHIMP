using System.Collections.Generic;

namespace Net.Chdk.Generators.Script
{
    public interface IScriptGenerator
    {
        void GenerateScript(string targetPath, string name, IDictionary<string, string> substitutes);
    }
}
