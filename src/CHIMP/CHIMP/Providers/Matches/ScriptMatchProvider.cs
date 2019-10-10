using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chimp.Model;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Substitute;

namespace Chimp.Providers.Matches
{
    sealed class ScriptMatchProvider : IMatchProvider
    {
        private ISubstituteProvider SubstituteProvider { get; }

        public ScriptMatchProvider(ISubstituteProvider substituteProvider)
        {
            SubstituteProvider = substituteProvider;
        }

        public Task<MatchData> GetMatchesAsync(SoftwareInfo software, string buildName, CancellationToken cancellationToken)
        {
            var substitutes = SubstituteProvider.GetSubstitutes(software);
            var result = GetMatches(substitutes);
            return Task.FromResult(result);
        }

        private static MatchData GetMatches(IDictionary<string, object> substitutes)
        {
            return new ScriptMatchData(substitutes);
        }
    }
}
