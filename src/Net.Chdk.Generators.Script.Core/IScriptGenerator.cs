using System.Collections.Generic;

namespace Net.Chdk.Generators.Script
{
    public interface IScriptGenerator
    {
        void GenerateScript(string filePath, string name, IDictionary<string, object> substitutes);
    }
}
