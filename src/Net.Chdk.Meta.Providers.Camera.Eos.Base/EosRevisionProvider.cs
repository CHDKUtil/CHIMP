using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    public abstract class EosRevisionProvider : ProductRevisionProvider<VersionData>
    {
        protected override string GetRevisionKey(string versionStr)
        {
            return $"{versionStr[0]}.{versionStr[1]}.{versionStr[2]}";
        }
    }
}
