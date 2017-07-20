using Net.Chdk.Model.Card;

namespace Chimp
{
    interface IFormatService
    {
        bool Format(CardInfo card, string fileSystem, string label);
    }
}
