using Net.Chdk.Model.Card;
using System.Collections.Generic;

namespace Net.Chdk.Generators.Script
{
    public interface IScriptGenerator
    {
        void GenerateScript(CardInfo cardInfo, string name, IDictionary<string, string> substitutes);
    }
}
