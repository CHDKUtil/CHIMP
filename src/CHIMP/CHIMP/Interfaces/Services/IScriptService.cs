using Net.Chdk.Model.Card;

namespace Chimp
{
    interface IScriptService
    {
        bool? TestScriptable(CardInfo cardInfo, string fileSystem);
        bool? SetScriptable(CardInfo cardInfo, string fileSystem, bool value);
    }
}
