using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Detectors.Software
{
    public interface IProductDetector
    {
        SoftwareProductInfo GetProduct(CardInfo cardInfo);
        string CategoryName { get; }
    }
}
