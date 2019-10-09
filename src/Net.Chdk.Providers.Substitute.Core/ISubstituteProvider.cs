using Net.Chdk.Model.Software;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Substitute
{
    public interface ISubstituteProvider
    {
        IDictionary<string, object>? GetSubstitutes(SoftwareInfo software);
    }
}
