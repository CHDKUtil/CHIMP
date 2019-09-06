using Net.Chdk.Model.Software;

namespace Net.Chdk.Meta.Providers.Software
{
    public interface ICompilerMetaProvider
    {
        SoftwareCompilerInfo GetCompiler(SoftwareInfo software);
    }
}
