using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Substitute;

namespace Net.Chdk.Providers.Software.Script
{
    public sealed class ScriptMatchProvider : IMatchProvider<ScriptMatchData>
    {
        private ISubstituteProvider SubstituteProvider { get; }

        public ScriptMatchProvider(ISubstituteProvider substituteProvider)
        {
            SubstituteProvider = substituteProvider;
        }

        public Task<ScriptMatchData> GetMatchesAsync(SoftwareInfo software, string buildName, CancellationToken cancellationToken)
        {
            var substitutes = SubstituteProvider.GetSubstitutes(software);
            var result = GetMatches(software, substitutes);
            return Task.FromResult(result);
        }

        private ScriptMatchData GetMatches(SoftwareInfo software, IDictionary<string, object>? substitutes)
        {
            if (substitutes == null)
                return new ScriptMatchData("Download_InvalidFormat_Text");
            if (!substitutes.ContainsKey("platform"))
            {
                var platforms = SubstituteProvider.GetSupportedPlatforms(software);
                return new ScriptMatchData(platforms: platforms);
            }
            if (!substitutes.ContainsKey("revision"))
            {
                var revisions = SubstituteProvider.GetSupportedRevisions(software);
                return new ScriptMatchData(revisions: revisions);
            }
            return new ScriptMatchData(substitutes);
        }
    }
}
