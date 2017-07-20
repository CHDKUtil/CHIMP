using Net.Chdk.Model.Software;

namespace Chimp.Providers.Build
{
    abstract class BuildProvider : IBuildProvider
    {
        public virtual string GetBuildName(SoftwareInfo softwareInfo)
        {
            return string.Empty;
        }
    }
}
