using Net.Chdk.Model.Software;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Substitute
{
    interface ICategorySubstituteProvider
    {
        IDictionary<string, object>? GetSubstitutes(SoftwareInfo software);
        IEnumerable<string> GetSupportedPlatforms(SoftwareInfo software);
    }
}
