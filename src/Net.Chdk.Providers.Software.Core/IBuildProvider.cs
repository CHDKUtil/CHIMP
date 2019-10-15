using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software
{
    public interface IBuildProvider
    {
        string GetBuildName(SoftwareInfo softwareInfo);
    }
}
