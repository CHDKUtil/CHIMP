using System.Text.RegularExpressions;
using Net.Chdk.Model.Software;

namespace Chimp
{
    interface ISoftwareProvider
    {
        SoftwareInfo GetSoftware(Match match, SoftwareModelInfo model);
    }
}
