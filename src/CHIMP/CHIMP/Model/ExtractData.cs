using System.Collections.Generic;

namespace Chimp.Model
{
    class ExtractData
    {
        public ExtractData(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }
    }

    sealed class ScriptExtractData : ExtractData
    {
        public ScriptExtractData(IDictionary<string, object> substitutes, string productName, string filePath)
            : base(filePath)
        {
            Substitutes = substitutes;
            ProductName = productName;
        }

        public IDictionary<string, object> Substitutes { get; }
        public string ProductName { get; }
    }
}
