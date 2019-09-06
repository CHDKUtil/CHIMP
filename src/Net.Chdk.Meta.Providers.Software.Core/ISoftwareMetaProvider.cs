using Net.Chdk.Model.Software;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Software
{
    public interface ISoftwareMetaProvider
    {
        IEnumerable<SoftwareInfo> GetSoftware(string path, string productName);
    }
}
