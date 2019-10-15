using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software
{
    public abstract class BuildProvider : IBuildProvider
    {
        public virtual string GetBuildName(SoftwareInfo softwareInfo)
        {
            return string.Empty;
        }
    }
}
