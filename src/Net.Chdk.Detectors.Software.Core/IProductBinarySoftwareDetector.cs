using Net.Chdk.Model.Software;

namespace Net.Chdk.Detectors.Software
{
    public interface IProductBinarySoftwareDetector
    {
        string CategoryName { get; }
        string ProductName { get; }
        byte[] Bytes { get; }
        SoftwareInfo GetSoftware(byte[] buffer, int index);
    }
}
