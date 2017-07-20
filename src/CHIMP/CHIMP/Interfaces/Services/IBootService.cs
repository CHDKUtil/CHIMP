using Net.Chdk.Model.Card;

namespace Chimp
{
    interface IBootService
    {
        string TestBootable(CardInfo cardInfo, string fileSystem);
        bool SetBootable(CardInfo cardInfo, string fileSystem, string categoryName, bool value);
    }
}
