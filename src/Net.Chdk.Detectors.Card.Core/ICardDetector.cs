using Net.Chdk.Model.Card;

namespace Net.Chdk.Detectors.Card
{
    public interface ICardDetector
    {
        CardInfo[] GetCards();
        CardInfo GetCard(string driveLetter);
    }
}
