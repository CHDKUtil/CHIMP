using Net.Chdk.Model.Software;

namespace Net.Chdk.Detectors.Software
{
    public interface IProductBinaryModuleDetector
    {
        string ProductName { get; }
        byte[] Bytes { get; }
        ModuleInfo GetModule(SoftwareInfo software, byte[] buffer, int index, string hashName);
    }
}
