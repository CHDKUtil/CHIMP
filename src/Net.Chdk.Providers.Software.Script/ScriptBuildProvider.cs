using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software.Script
{
    public sealed class ScriptBuildProvider : IBuildProvider
    {
        public string GetBuildName(SoftwareInfo softwareInfo)
        {
            return string.Empty;
        }
    }
}
