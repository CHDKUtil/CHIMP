using Net.Chdk.Model.Software;

namespace Net.Chdk.Detectors.Software
{
    public interface IInnerModuleDetector
    {
        ModuleInfo GetModule(SoftwareInfo software, byte[] buffer, string hashName);
    }
}
