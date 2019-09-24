using Chimp.Model;
using Chimp.Providers.Matches;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;

namespace Chimp.Resolvers
{
    sealed class MatchProviderResolver : ProviderResolver<IMatchProvider, MatchProvider>
    {
        public MatchProviderResolver(IServiceActivator serviceActivator, IDictionary<string, Distro>? distros)
            : base(serviceActivator, distros)
        {
        }

        protected override string? GetTypeName(Distro distro)
        {
            return distro.MatchType;
        }

        protected override IEnumerable<Type> GetTypes()
        {
            yield return typeof(Uri);
            yield return typeof(IDictionary<string, string>);
        }

        protected override IEnumerable<object> GetValues(string sourceName, SoftwareSourceInfo source, Distro distro)
        {
            yield return distro.BaseUrl!;
            yield return distro.Builds!;
        }
    }
}
