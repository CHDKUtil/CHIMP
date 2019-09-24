using Chimp.Model;
using Chimp.Providers.Build;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;

namespace Chimp.Resolvers
{
    sealed class BuildProviderResolver : ProviderResolver<IBuildProvider, BuildProvider>
    {
        public BuildProviderResolver(IServiceActivator serviceActivator, IDictionary<string, Distro>? distros)
            : base(serviceActivator, distros)
        {
        }

        protected override string? GetTypeName(Distro distro)
        {
            return distro.ProductType;
        }

        protected override IEnumerable<Type> GetTypes()
        {
            yield return typeof(SoftwareSourceInfo);
        }

        protected override IEnumerable<object> GetValues(string sourceName, SoftwareSourceInfo source, Distro distro)
        {
            yield return source;
        }
    }
}
