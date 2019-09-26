using Net.Chdk.Meta.Providers;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Substitute
{
    interface ICategorySubstituteProvider
    {
        IDictionary<string, string>? GetSubstitutes(string platform, string revision);
    }
}
