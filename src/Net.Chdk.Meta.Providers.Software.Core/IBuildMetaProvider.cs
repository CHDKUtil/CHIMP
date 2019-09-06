using Net.Chdk.Model.Software;

namespace Net.Chdk.Meta.Providers.Software
{
    public interface IBuildMetaProvider
    {
        SoftwareBuildInfo GetBuild(SoftwareInfo software);
    }
}
