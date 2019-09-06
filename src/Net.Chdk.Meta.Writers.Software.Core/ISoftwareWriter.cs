using System.Collections.Generic;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Meta.Writers.Software
{
    public interface ISoftwareWriter
    {
        void WriteSoftware(string path, IDictionary<string, SoftwareInfo> hash2sw);
    }
}
