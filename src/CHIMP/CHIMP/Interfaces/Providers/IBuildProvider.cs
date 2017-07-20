using Net.Chdk.Model.Software;

namespace Chimp
{
    interface IBuildProvider
    {
        string GetBuildName(SoftwareInfo softwareInfo);
    }
}